//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;
//using KosmoForum.DbContext;
//using KosmoForum.Models;
//using KosmoForum.Repository;
//using KosmoForum.Repository.IRepository;
//using Microsoft.EntityFrameworkCore;
//using Xunit;
//using Moq;

//namespace KosmoForumTests.Repository
//{
//    public class CategoryRepoTest
//    {
//        private readonly ICategoryRepo _categoryRepo;
//        private readonly Mock<ApplicationDbContext> _mockDbContext;
        
//        private readonly Mock<DbSet<Category>> _mockDbSetCategory;
//        public CategoryRepoTest()
//        {
//            _mockDbContext = new Mock<ApplicationDbContext>();
//            _mockDbSetCategory = new Mock<DbSet<Category>>();
//            _mockDbContext.Setup(x => x.Set<Category>()).Returns(_mockDbSetCategory.Object);
//            _mockDbSetCategory.Setup(x => x.Find(It.IsAny<int>())).Returns(new Category());
//            _categoryRepo = new CategoryRepo(_mockDbContext.Object);
//        }

//        [Fact]
//        public void GetCategoryWithIdShouldReturnCategoryObject()
//        {
//            var result = _categoryRepo.GetCategory(2);


//        }
//    }
//}
