using MyBlog.Data;
using MyBlog.Data.UnitOfWork;
using MyBlog.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Service
{
    public class BlogService : BaseService
    {
        public List<BlogDTO> GetBlogs()
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                try
                {
                    var result = uow.Blogs
                        .List()
                        .OrderByDescending(x => x.CreatedDate)
                        .ToList();

                    List<BlogDTO> list = new List<BlogDTO>();

                    foreach (var item in result)
                    {
                        BlogDTO obj = new BlogDTO
                        {
                            BlogId = item.BlogId,
                            Title = item.Title,
                            CategoryName = item.Category.CategoryName,
                            BlogContent = item.BlogContent.Length>=250 ? item.BlogContent.Substring(0, 250) : item.BlogContent,
                            CreatedDate = item.CreatedDate,
                            ImagePath=item.ImagePath,
                            RecordStatusName=item.RecordStatus.RecordStatusName
                        };

                        list.Add(obj);
                    }

                    return list;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public BlogDTO Get(int id)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                try
                {
                    var result = uow.Blogs.Get(x =>
                            x.BlogId == id && x.IsConfirmed);

                    if (result == null)
                    {
                        return null;
                    }

                    BlogDTO blog = Mapper.Map<Blog, BlogDTO>(result);

                    List<CommentDTO> list = new List<CommentDTO>();

                    foreach (var item in result.Comments)
                    {
                        CommentDTO cmd = new CommentDTO
                        {
                            BlogId = item.BlogId,
                            CommentContent = item.CommentContent,
                            CommentId = item.CommentId,
                            CreatedDate = item.CreatedDate,
                            IsConfirmed = item.IsConfirmed,
                            UserId = item.UserId,
                            UserName = item.User.FirstName + " " + item.User.LastName
                        };
                        list.Add(cmd);
                    }

                    blog.CommandList = list;
                    blog.CategoryName = result.Category.CategoryName;
                    blog.Author = result.User.FirstName + " " + result.User.LastName;
                    blog.RecordStatusName = result.RecordStatus.RecordStatusName;

                    return blog;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public int Save(BlogDTO obj)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                try
                {
                    obj.IsConfirmed = false;
                    obj.RecordStatusId = Convert.ToByte(Enums.RecordStatus.Aktif);
                    obj.CreatedDate = DateTime.Now;

                    var entity = uow.Blogs.Save(Mapper.Map<BlogDTO, Blog>(obj));

                    uow.Commit();

                    return entity.BlogId;
                }
                catch (Exception ex)
                {
                    uow.RollBack();
                    return 0;
                }
            }
        }

        public bool Confirm(int id)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                try
                {
                    var entity = uow.Blogs.Get(id);
                    entity.IsConfirmed = true;
                    uow.Blogs.Update(entity);

                    return uow.Commit();
                }
                catch (Exception ex)
                {
                    uow.RollBack();
                    return false;
                }
            }
        }

        public bool Update(BlogDTO obj)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                try
                {
                    var entity = uow.Blogs.Update(Mapper.Map<BlogDTO, Blog>(obj));

                    return uow.Commit();
                }
                catch (Exception)
                {
                    uow.RollBack();
                    return false;
                }
            }
        }

        public List<BlogDTO> GetBlogsByCategory(int id)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                try
                {
                    var result = uow.Blogs
                        .Search(x => x.CategoryId == id)
                        .OrderByDescending(x => x.CreatedDate)
                        .ToList();

                    List<BlogDTO> list = new List<BlogDTO>();

                    foreach (var item in result)
                    {
                        BlogDTO obj = new BlogDTO
                        {
                            BlogId = item.BlogId,
                            Title = item.Title,
                            CategoryName = item.Category.CategoryName,
                            BlogContent = item.BlogContent.Substring(0, 250),
                            CreatedDate = item.CreatedDate
                        };

                        list.Add(obj);
                    }

                    return list;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
    }
}
