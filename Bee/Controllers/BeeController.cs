using Bee.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.Data.Entity;
using System.IO;
using System.Net.Mail;

namespace Bee.Controllers
{
    public class BeeController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Test()
        {
            return Ok("Test New");
        }

        [HttpPost]
        public IHttpActionResult Init(int id, [FromBody] string token)
        {
            User user;

            using (AppDbContext _db = new AppDbContext())
            {
                user = _db.Users.Where(u => u.Id == id).FirstOrDefault();
                int friendNotif = _db.Friends.Where(f => f.To.Id == id && f.Status == "pending").Count();

                if (!user.IsInvisible && user != null)
                {
                    user.IsOnline = true;
                    _db.SaveChanges();
                }

                return Ok(new { User = user, IsMember = Authenticate(id, token), FriendNotif = friendNotif });
            }
        }

        [HttpPost]
        public IHttpActionResult Start(int id, [FromBody] string token)
        {
            User user;

            using (AppDbContext _db = new AppDbContext())
            {
                user = _db.Users.Where(u => u.Id == id).FirstOrDefault();

                if (user == null)
                {
                    return BadRequest("User not found.");
                }
                else
                {
                    List<string> groups = new List<string>();
                    groups = _db.Groups.Where(g => g.User.Id == id).Select(g => g.Title).ToList(); ;

                    var temp_p = _db.Posts.Where(p => p.Owner.Id == id || groups.Contains(p.Group)).Include(p => p.Author).OrderByDescending(p => p.Created);
                    List<Post> posts = new List<Post>();
                    List<Comment> comments = new List<Comment>();
                    List<Comment> temp_c = new List<Comment>();

                    int pictureCount = Array.FindAll(Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + @"users\" + id), (f) => !f.Contains("avatar.png")).ToArray().Length;
                    int friendCount = _db.Friends.Where(f => (f.To.Id == id || f.From.Id == id) && f.Status == "accepted").Select(f => (f.From.Id == id) ? f.To : f.From).Count();
                    int postCount = _db.Posts.Where(p => p.Owner.Id == id).Count();

                    foreach (Post p in temp_p)
                    {
                        posts.Add(p);
                    }

                    foreach (Post p in posts)
                    {
                       temp_c.AddRange(_db.Comments.Include("Author").Where(c => c.Post.Id == p.Id).ToList());
                    }

                    foreach(Comment c in temp_c)
                    {
                        comments.Add(c);
                    }

                    return Ok(new { Posts = (!user.PrivatePosts || Authenticate(id, token) ? posts : null), Comments = (!user.PrivatePosts || Authenticate(id, token) ? comments : null), Groups = (Authenticate(id, token) ? groups : null), FriendCount = friendCount, PictureCount = pictureCount, PostCount = postCount });
                }
            }
        }

        [HttpPost]
        public IHttpActionResult Groups(int id, [FromBody] string token)
        {
            using (AppDbContext _db = new AppDbContext())
            {
                if(Authenticate(id, token))
                {
                    IList<string> member = _db.Groups.Where(g => g.User.Id == id).Select(g => g.Title).ToList();
                    var result = _db.Groups.GroupBy(g => g.Title, g => g, (key, g) => new { Group = key, Count = g.Count() - 1 }).ToDictionary(g => g.Group, g => g.Count);

                    return Ok(new { Groups = result, Member = member });
                }
                else
                {
                    return BadRequest("Invalid input.");
                }
            }
        }

        [HttpPost]
        public IHttpActionResult Pictures(int id, [FromBody] string token)
        {
            User user = new User();

            using (AppDbContext _db = new AppDbContext())
            {
                user = _db.Users.Where(u => u.Id == id).FirstOrDefault();

                if(user != null && (Authenticate(id, token) || !user.PrivatePictures))
                {
                    string[] files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + @"users\" + id);

                    for(int i = 0; i < files.Length; i++)
                    {
                        files[i] = files[i].Substring(files[i].LastIndexOf("\\") + 1);
                    }

                    files = Array.FindAll(files, (f) => !f.Contains("avatar.png")).ToArray();

                    return Ok(new { Pictures = files });
                }
                else
                {
                    return BadRequest("Invalid input.");
                }
            }
        }

        [HttpPost]
        public IHttpActionResult Friends(int id, [FromBody] string token)
        {
            User user = new User();

            using (AppDbContext _db = new AppDbContext())
            {
                user = _db.Users.Where(u => u.Id == id).FirstOrDefault();

                if (user != null && (Authenticate(id, token) || !user.PrivateFriends))
                {
                    IList<Friend> friends = _db.Friends.Where(f => f.From.Id == id || f.To.Id == id).Include(f => f.From).Include(f => f.To).ToList();

                    return Ok(new { Friends = friends });
                }
                else
                {
                    return BadRequest("Invalid input.");
                }
            }
        }

        [HttpPost]
        public IHttpActionResult Profile(int id)
        {
            User user = new User();
            List<User> friends = new List<User>();
            string[] pictures;

            using (AppDbContext _db = new AppDbContext())
            {
                user = _db.Users.Where(u => u.Id == id).FirstOrDefault();
                friends = user.PrivateFriends ? null : _db.Friends.Where(f => (f.To.Id == id || f.From.Id == id) && f.Status == "accepted").Select(f => (f.From.Id == id) ? f.To : f.From).Take(8).ToList();
                pictures = user.PrivatePictures ? new string[0] : Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + @"users\" + id);
                int postCount = _db.Posts.Where(p => p.Owner.Id == id).Count();
                int friendCount = user.PrivateFriends ? 0 : _db.Friends.Where(f => (f.To.Id == id || f.From.Id == id) && f.Status == "accepted").Select(f => (f.From.Id == id) ? f.To : f.From).Count();

                for (int i = 0; i < pictures.Length; i++)
                {
                    pictures[i] = pictures[i].Substring(pictures[i].LastIndexOf("\\") + 1);
                }

                pictures = Array.FindAll(pictures, (f) => !f.Contains("avatar.png")).ToArray();

                if(user != null)
                {
                    return Ok(new { User = user, Friends = friends, Pictures = pictures, PostCount = postCount, FriendCount = friendCount });
                }
                else
                {
                    return BadRequest("Invalid input.");
                }
            }
        }

        [HttpPost]
        public IHttpActionResult Messages(int id, [FromBody] string token)
        {
            User user = new User();

            using (AppDbContext _db = new AppDbContext())
            {
                user = _db.Users.Where(u => u.Id == id).FirstOrDefault();

                if (user != null && (Authenticate(id, token) || !user.PrivatePictures))
                {
                    IList<User> friends = _db.Friends.Where(f => f.From.Id == id || f.To.Id == id).Include(f => f.From).Include(f => f.To).Select(f => (f.From.Id == id) ? f.To : f.From).ToList();
                    ISet<int> haveNew = new HashSet<int>(_db.Messages.Where(m => m.To.Id == id && m.Status == "unread").Select(m => (m.From.Id == id) ? m.To.Id : m.From.Id).ToList());

                    return Ok(new { Friends = friends, HaveNew = haveNew });
                }
                else
                {
                    return BadRequest("Invalid input.");
                }
            }
        }

        [HttpPost]
        public IHttpActionResult CV(int id, [FromBody] string token)
        {
            User user = new User();

            using (AppDbContext _db = new AppDbContext())
            {
                user = _db.Users.Where(u => u.Id == id).FirstOrDefault();

                if (user != null && (Authenticate(id, token) || !user.PrivateCV))
                {
                    IList<Work> work = _db.Works.Where(w => w.User.Id == id).ToList();
                    IList<Education> education = _db.Educations.Where(e => e.User.Id == id).ToList();
                    IList<Skill> skills = _db.Skills.Where(s => s.User.Id == id).ToList();
                    IList<Language> languages = _db.Languages.Where(l => l.User.Id == id).ToList();

                    return Ok(new { Work = work, Education = education, Skills = skills, Languages = languages });
                }
                else
                {
                    return BadRequest("Invalid input.");
                }
            }
        }

        [HttpPost]
        public IHttpActionResult Search(int id, [FromBody] string term)
        {
            using (AppDbContext _db = new AppDbContext())
            {
                IList<User> result = _db.Users.Where(u => ((u.FirstName + " " + u.LastName).ToLower().StartsWith(term.ToLower()) || (u.LastName + " " + u.FirstName).ToLower().StartsWith(term.ToLower()))).ToList();

                return Ok(new { Results = result });
            }
        }

        [HttpPost]
        public IHttpActionResult AddPost(int id, [FromBody] NewPost model)
        {
            Post post = new Post();

            using (AppDbContext _db = new AppDbContext())
            {
                if(!Authenticate(id, model.token) || string.IsNullOrEmpty(model.Content) || string.IsNullOrWhiteSpace(model.Content))
                {
                    return BadRequest("Invalid input.");
                }

                post.Author = _db.Users.Where(u => u.Id == model.AuthorId).FirstOrDefault();
                post.Owner = _db.Users.Where(u => u.Id == id).FirstOrDefault();
                post.Created = DateTime.Now;
                post.Content = model.Content.Replace("<", string.Empty).Replace(">", string.Empty);
                post.Group = model.Group;
                post.Type = model.Type;
                post.Likes = 0;

                _db.Posts.Add(post);
                _db.SaveChanges();

                return Ok();
            }
        }

        [HttpPost]
        public IHttpActionResult AddComment(int id, [FromBody] NewComment model)
        {
            Comment comment = new Comment();

            using (AppDbContext _db = new AppDbContext())
            {
                if (!Authenticate(id, model.token) || string.IsNullOrEmpty(model.Content) || string.IsNullOrWhiteSpace(model.Content))
                {
                    return BadRequest("Invalid input.");
                }

                comment.Author = _db.Users.Where(u => u.Id == model.AuthorId).FirstOrDefault();
                comment.Post = _db.Posts.Where(p => p.Id == model.PostId).FirstOrDefault();
                comment.Created = DateTime.Now;
                comment.Content = model.Content.Replace("<", string.Empty).Replace(">", string.Empty);

                _db.Comments.Add(comment);
                _db.SaveChanges();

                return Ok();
            }
        }

        [HttpPost]
        public IHttpActionResult Like(int id, [FromBody] NewLike model)
        {
            Like like = new Like();

            using (AppDbContext _db = new AppDbContext())
            {
                if(Authenticate(id, model.token) && _db.Likes.Where(l => l.User.Id == model.UserId && l.Post.Id == model.PostId).FirstOrDefault() == null)
                {
                    like.Post = _db.Posts.Where(p => p.Id == model.PostId).FirstOrDefault();
                    like.User = _db.Users.Where(u => u.Id == model.UserId).FirstOrDefault();

                    _db.Likes.Add(like);
                    _db.Posts.Where(p => p.Id == model.PostId).FirstOrDefault().Likes++;

                    _db.SaveChanges();

                    return Ok();
                }
                else
                {
                    return BadRequest("Invalid input.");
                }
            }
        }

        [HttpPost]
        public IHttpActionResult DeletePost(int id, [FromBody] DeletePost model)
        {
            Post post = new Post();

            using (AppDbContext _db = new AppDbContext())
            {
                post = _db.Posts.Where(p => p.Id == model.PostId).Include(p => p.Owner).FirstOrDefault();

                if(Authenticate(id, model.token) && post != null && post.Owner.Id == id)
                {
                    _db.Posts.Remove(_db.Posts.Where(p => p.Id == model.PostId).FirstOrDefault());
                    _db.SaveChanges();

                    return Ok();
                }
                else
                {
                    return BadRequest("Invalid input.");
                }
            }
        }

        [HttpPost]
        public IHttpActionResult SharePost(int id, [FromBody] SharePost model)
        {
            Post post = new Post();

            using (AppDbContext _db = new AppDbContext())
            {
                post = _db.Posts.Where(p => p.Id == model.PostId).Include(p => p.Owner).FirstOrDefault();

                if (Authenticate(id, model.token) && post != null && post.Owner.Id != id && post.Author.Id != id)
                {
                    post.Owner = _db.Users.Where(u => u.Id == id).FirstOrDefault();
                    post.Created = DateTime.Now;

                    _db.Posts.Add(post);
                    _db.SaveChanges();

                    return Ok();
                }
                else
                {
                    return BadRequest("Invalid input.");
                }
            }
        }

        [HttpPost]
        public IHttpActionResult JoinGroup(int id, [FromBody] NewGroup model)
        {
            Group group;

            using (AppDbContext _db = new AppDbContext())
            {
                group = new Group()
                {
                    Title = model.Title,
                    User = _db.Users.Where(u => u.Id == id).FirstOrDefault()
                };

                if (Authenticate(id, model.token) && _db.Groups.Where(g => g.User.Id == id && g.Title == model.Title).FirstOrDefault() == null)
                {
                    _db.Groups.Add(group);
                    _db.SaveChanges();

                    return Ok();
                }
                else
                {
                    return BadRequest("Invalid input.");
                }
            }
        }

        [HttpPost]
        public IHttpActionResult ExitGroup(int id, [FromBody] NewGroup model)
        {
            Group group;

            using (AppDbContext _db = new AppDbContext())
            {
                group = _db.Groups.Where(g => g.User.Id == id && g.Title == model.Title).FirstOrDefault();

                if (Authenticate(id, model.token) && group != null)
                {
                    _db.Groups.Remove(group);
                    _db.SaveChanges();

                    return Ok();
                }
                else
                {
                    return BadRequest("Invalid input.");
                }
            }
        }

        [HttpPost]
        public IHttpActionResult DeletePicture(int id, [FromBody] DeletePicture model)
        {
            using (AppDbContext _db = new AppDbContext())
            {
                if (Authenticate(id, model.token) && model.FileName != "avatar.png")
                {
                    try
                    {
                        File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"users\" + id + "\\" + model.FileName);
                    }
                    catch
                    {
                        return BadRequest("Invalid input.");
                    }

                    _db.Posts.RemoveRange(_db.Posts.Where(p => p.Content == model.FileName));
                    _db.SaveChanges();

                    return Ok();
                }
                else
                {
                    return BadRequest("Invalid input.");
                }
            }
        }

        [HttpPost]
        public IHttpActionResult AddFriend(int id, [FromBody] NewFriend model)
        {
            Friend friend;

            using (AppDbContext _db = new AppDbContext())
            {
                friend = _db.Friends.Where(f => (f.From.Id == model.UserId && f.To.Id == id) || (f.To.Id == model.UserId && f.From.Id == id)).FirstOrDefault();

                if (Authenticate(id, model.token) && friend == null)
                {
                    friend = new Friend();

                    friend.Status = "pending";
                    friend.From = _db.Users.Where(u => u.Id == id).FirstOrDefault();
                    friend.To = _db.Users.Where(u => u.Id == model.UserId).FirstOrDefault();

                    _db.Friends.Add(friend);
                    _db.SaveChanges();

                    return Ok();
                }
                else
                {
                    return BadRequest("Invalid input.");
                }
            }
        }

        [HttpPost]
        public IHttpActionResult AcceptFriend(int id, [FromBody] NewFriend model)
        {
            Friend friend = new Friend();

            using (AppDbContext _db = new AppDbContext())
            {
                friend = _db.Friends.Where(f => f.From.Id == model.UserId && f.To.Id == id && f.Status == "pending").FirstOrDefault();

                if (Authenticate(id, model.token) && friend != null)
                {
                    friend.Status = "accepted";

                    _db.SaveChanges();

                    return Ok();
                }
                else
                {
                    return BadRequest("Invalid input.");
                }
            }
        }

        [HttpPost]
        public IHttpActionResult DeclineFriend(int id, [FromBody] NewFriend model)
        {
            Friend friend = new Friend();

            using (AppDbContext _db = new AppDbContext())
            {
                friend = _db.Friends.Where(f => f.From.Id == model.UserId && f.To.Id == id && f.Status == "pending").FirstOrDefault();

                if (Authenticate(id, model.token) && friend != null)
                {
                    _db.Friends.Remove(friend);
                    _db.SaveChanges();

                    return Ok();
                }
                else
                {
                    return BadRequest("Invalid input.");
                }
            }
        }

        [HttpPost]
        public IHttpActionResult DeleteFriend(int id, [FromBody] NewFriend model)
        {
            Friend friend = new Friend();

            using (AppDbContext _db = new AppDbContext())
            {
                friend = _db.Friends.Where(f => (f.From.Id == model.UserId || f.From.Id == id) && (f.To.Id == model.UserId || f.To.Id == id) && f.Status == "accepted").FirstOrDefault();

                if (Authenticate(id, model.token) && friend != null)
                {
                    _db.Friends.Remove(friend);
                    _db.SaveChanges();

                    return Ok();
                }
                else
                {
                    return BadRequest("Invalid input.");
                }
            }
        }

        [HttpPost]
        public IHttpActionResult GetDefinition(int id, [FromBody] string term)
        {
            WebClient client = new WebClient();
            string result = client.DownloadString("https://en.wikipedia.org/w/api.php?format=json&action=query&prop=extracts&exintro&explaintext&redirects=1&titles=" + term);

            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult PageInfo(int id)
        {
            User user = new Models.User();

            using (AppDbContext _db = new AppDbContext())
            {
                user = _db.Users.Where(u => u.Id == id).FirstOrDefault();
                return Ok(new { User = user });
            }
        }

        [HttpPost]
        public IHttpActionResult GetMessages(int id, [FromBody] NewMessage model)
        {
            using (AppDbContext _db = new AppDbContext())
            {
                if (Authenticate(id, model.token))
                {
                    IList<Message> messages = _db.Messages.Where(m => (m.From.Id == id || m.To.Id == id) && (m.From.Id == model.UserId || m.To.Id == model.UserId)).Include(m => m.From).Include(m => m.To).OrderByDescending(m => m.Created).ToList();

                    foreach (Message m in _db.Messages.Where(m => (m.From.Id == model.UserId && m.To.Id == id)))
                    {
                        m.Status = "read";
                    }

                    _db.SaveChanges();

                    return Ok(new { Messages = messages });
                }
                else
                {
                    return BadRequest("Invalid input.");
                }
            }
        }

        [HttpPost]
        public IHttpActionResult AddMessage(int id, [FromBody] NewMessage model)
        {
            Message message = new Message();

            using (AppDbContext _db = new AppDbContext())
            {
                if (Authenticate(id, model.token) && !string.IsNullOrEmpty(model.Content) && !string.IsNullOrWhiteSpace(model.Content))
                {
                    User from = _db.Users.Where(u => u.Id == id).FirstOrDefault();
                    User to = _db.Users.Where(u => u.Id == model.UserId).FirstOrDefault();

                    message.From = from;
                    message.To = to;
                    message.Content = model.Content.Replace("<", string.Empty).Replace(">", string.Empty);
                    message.Status = "unread";
                    message.Created = DateTime.Now;

                    _db.Messages.Add(message);
                    _db.SaveChanges();

                    return Ok();
                }
                else
                {
                    return BadRequest("Invalid input.");
                }
            }
        }

        [HttpPost]
        public IHttpActionResult AddWork(int id, [FromBody] NewWork model)
        {
            Work work = new Work();

            using (AppDbContext _db = new AppDbContext())
            {
                if (!Authenticate(id, model.token))
                {
                    return BadRequest("Invalid input.");
                }

                work.User = _db.Users.Where(u => u.Id == id).FirstOrDefault();
                work.Title = model.Title;
                work.From = model.From;
                work.To = model.To;
                work.Company = model.Company;
                work.Size = model.Size;

                _db.Works.Add(work);
                _db.SaveChanges();

                return Ok();
            }
        }

        [HttpPost]
        public IHttpActionResult DeleteWork(int id, [FromBody] NewWork model)
        {
            Work work = new Work();

            using (AppDbContext _db = new AppDbContext())
            {
                work = _db.Works.Where(w => w.Id == model.WorkId).Include(w => w.User).FirstOrDefault();

                if (!Authenticate(id, model.token) || !(work.User.Id == id))
                {
                    return BadRequest("Invalid input.");
                }

                _db.Works.Remove(work);
                _db.SaveChanges();

                return Ok();
            }
        }

        [HttpPost]
        public IHttpActionResult AddEducation(int id, [FromBody] NewEducation model)
        {
            Education education = new Education();

            using (AppDbContext _db = new AppDbContext())
            {
                if (!Authenticate(id, model.token))
                {
                    return BadRequest("Invalid input.");
                }

                education.User = _db.Users.Where(u => u.Id == id).FirstOrDefault();
                education.Title = model.Title;
                education.From = model.From;
                education.To = model.To;
                education.Institute = model.Institute;
                education.Description = model.Description;

                _db.Educations.Add(education);
                _db.SaveChanges();

                return Ok();
            }
        }

        [HttpPost]
        public IHttpActionResult DeleteEducation(int id, [FromBody] NewEducation model)
        {
            Education education = new Education();

            using (AppDbContext _db = new AppDbContext())
            {
                education = _db.Educations.Where(e => e.Id == model.EducationId).Include(e => e.User).FirstOrDefault();

                if (!Authenticate(id, model.token) || !(education.User.Id == id))
                {
                    return BadRequest("Invalid input.");
                }

                _db.Educations.Remove(education);
                _db.SaveChanges();

                return Ok();
            }
        }

        [HttpPost]
        public IHttpActionResult AddSkill(int id, [FromBody] NewSkill model)
        {
            Skill skill = new Skill();

            using (AppDbContext _db = new AppDbContext())
            {
                if (!Authenticate(id, model.token))
                {
                    return BadRequest("Invalid input.");
                }

                skill.User = _db.Users.Where(u => u.Id == id).FirstOrDefault();
                skill.Title = model.Title;

                _db.Skills.Add(skill);
                _db.SaveChanges();

                return Ok();
            }
        }

        [HttpPost]
        public IHttpActionResult DeleteSkill(int id, [FromBody] NewSkill model)
        {
            Skill skill = new Skill();

            using (AppDbContext _db = new AppDbContext())
            {
                skill = _db.Skills.Where(s => s.Id == model.SkillId).Include(e => e.User).FirstOrDefault();

                if (!Authenticate(id, model.token) || !(skill.User.Id == id))
                {
                    return BadRequest("Invalid input.");
                }

                _db.Skills.Remove(skill);
                _db.SaveChanges();

                return Ok();
            }
        }

        [HttpPost]
        public IHttpActionResult AddLanguage(int id, [FromBody] NewLanguage model)
        {
            Language language = new Language();

            using (AppDbContext _db = new AppDbContext())
            {
                if (!Authenticate(id, model.token))
                {
                    return BadRequest("Invalid input.");
                }

                language.User = _db.Users.Where(u => u.Id == id).FirstOrDefault();
                language.Title = model.Title;
                language.Level = model.Level;

                _db.Languages.Add(language);
                _db.SaveChanges();

                return Ok();
            }
        }

        [HttpPost]
        public IHttpActionResult DeleteLanguage(int id, [FromBody] NewLanguage model)
        {
            Language language = new Language();

            using (AppDbContext _db = new AppDbContext())
            {
                language = _db.Languages.Where(l => l.Id == model.LanguageId).Include(e => e.User).FirstOrDefault();

                if (!Authenticate(id, model.token) || !(language.User.Id == id))
                {
                    return BadRequest("Invalid input.");
                }

                _db.Languages.Remove(language);
                _db.SaveChanges();

                return Ok();
            }
        }

        [HttpGet]
        public IHttpActionResult GetNews(string category)
        {
            WebClient client = new WebClient();
            client.Headers.Add("Accept-Language", "en-US");
            client.Headers.Add("Accept", "*/*");
            client.Headers.Add("User-Agent", "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)");
            string result = client.DownloadString(new Uri($"https://newsapi.org/v2/top-headlines?sources=bbc-news&apiKey=e985391e791446bba816fc6b49a25fd6"));
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(result, System.Text.Encoding.UTF8, "application/json");
            return ResponseMessage(response);
        }

        [HttpPost]
        public IHttpActionResult UpdateProfileSettings(int id, [FromBody] ProfileSettings model)
        {
            User user;

            using (AppDbContext _db = new AppDbContext())
            {
                user = _db.Users.Where(u => u.Id == id).FirstOrDefault();

                if(user == null || !Authenticate(id, model.token))
                {
                    return BadRequest("User not found.");
                }

                user.City = model.City;
                user.Country = model.Country;
                user.CityOfBirth = model.CityOfBirth;
                user.CountryOfBirth = model.CountryOfBirth;
                user.Birthdate = model.Birthdate;
                user.Occupation = model.Occupation;
                user.Work = model.Work;
                user.College = model.College;
                user.School = model.School;
                user.Relationship = model.Relationship;
                user.Website = model.Website;

                _db.SaveChanges();

                return Ok();
            }
        }

        [HttpPost]
        public IHttpActionResult UpdateSettings(int id, [FromBody] Settings model)
        {
            User user;
            string token;

            using (AppDbContext _db = new AppDbContext())
            {
                user = _db.Users.Where(u => u.Id == id).FirstOrDefault();

                if (user == null || !Authenticate(id, model.token))
                {
                    return BadRequest("User not found.");
                }

                using (SHA384 sha = new SHA384Managed())
                {
                    if (!string.IsNullOrEmpty(model.Email) && !string.IsNullOrWhiteSpace(model.Email) && model.Email == model.ReEmail)
                    {
                        user.Email = model.Email;
                    }

                    if (model.Password.Length >= 6 && !string.IsNullOrWhiteSpace(model.Password) && model.Password == model.RePassword)
                    {
                        user.Password = BitConverter.ToString(sha.ComputeHash(Encoding.UTF8.GetBytes(model.Password))).Replace("-", string.Empty).ToLower();
                    }


                    token = BitConverter.ToString(sha.ComputeHash(Encoding.UTF8.GetBytes(user.Password + user.Email))).Replace("-", string.Empty).ToLower();
                }

                user.FirstName = (string.IsNullOrEmpty(model.FirstName) || string.IsNullOrWhiteSpace(model.FirstName) ? user.FirstName : model.FirstName);
                user.LastName = (string.IsNullOrEmpty(model.LastName) || string.IsNullOrWhiteSpace(model.LastName) ? user.LastName : model.LastName);
                user.IsInvisible = model.IsInvisible;

                if (user.IsInvisible)
                {
                    user.IsOnline = false;
                }
                else
                {
                    user.IsOnline = true;
                }

                _db.SaveChanges();

                return Ok(new { token = token });
            }
        }

        [HttpPost]
        public IHttpActionResult UpdatePrivacySettings(int id, [FromBody] PrivacySettings model)
        {
            User user;

            using (AppDbContext _db = new AppDbContext())
            {
                user = _db.Users.Where(u => u.Id == id).FirstOrDefault();

                if (user == null || !Authenticate(id, model.token))
                {
                    return BadRequest("User not found.");
                }

                user.PrivatePosts = model.PrivatePosts;
                user.PrivateFriends = model.PrivateFriends;
                user.PrivatePictures = model.PrivatePictures;
                user.PrivateCV = model.PrivateCV;
                user.PrivateEmail = model.PrivateEmail;

                _db.SaveChanges();

                return Ok();
            }
        }

        [HttpGet]
        public IHttpActionResult SetOffLine(int id)
        {
            User user = new Models.User();

            using (AppDbContext _db = new AppDbContext())
            {
                user = _db.Users.Where(u => u.Id == id).FirstOrDefault();

                if(user != null)
                {
                    user.IsOnline = false;
                    _db.SaveChanges();

                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
        }

        [HttpGet]
        public IHttpActionResult ResetPassword(string email)
        {
            string password = DateTime.Now.Millisecond.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Ticks.ToString().Substring(0, 4);

            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            mail.To.Add(email);
            mail.From = new MailAddress("joingnomi@outlook.com", "The GNOMI Team", System.Text.Encoding.UTF8);
            mail.Subject = "Your new password.";
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = "Your new password: " + password; ;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            SmtpClient smtp = new SmtpClient("smtp-mail.outlook.com", 587);
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("joingnomi@outlook.com", "$Papajoe66");

            try
            {
                using (AppDbContext _db = new AppDbContext())
                {
                    User user = _db.Users.Where(u => u.Email == email).FirstOrDefault();

                    using (SHA384 sha = new SHA384Managed())
                    {
                        password = BitConverter.ToString(sha.ComputeHash(Encoding.UTF8.GetBytes(password))).Replace("-", string.Empty).ToLower();
                    }

                    user.Password = password;
                    _db.SaveChanges();
                }

                smtp.Send(mail);

                return Ok();
            }
            catch
            {
                return BadRequest("Invalid input.");
            }

        }

        [NonAction]
        private bool Authenticate(int id, string token)
        {
            User user;

            using (AppDbContext _db = new AppDbContext())
            {
                user = _db.Users.Where(u => u.Id == id).FirstOrDefault();

                if(user == null)
                {
                    return false;
                }

                using (SHA384 sha = new SHA384Managed())
                {
                    if(BitConverter.ToString(sha.ComputeHash(Encoding.UTF8.GetBytes(user.Password + user.Email))).Replace("-", string.Empty).ToLower() == token)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
    }
}
