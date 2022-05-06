using Application.DTO.User;
using Application.Profiles;
using Application.Response;
using AutoMapper;
using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using UserManager.Controllers;
using Xunit;

namespace UserManager.UnitTests
{
    public class UserControllerTests : IDisposable
    {
        UserProfile _userProfile;
        UserAdd _registerUser = new();
        MapperConfiguration _configuration;
        User _commonUser = new();
        Mock<IUser> _mockRepo;
        IMapper _mapper;

        public UserControllerTests()
        {
            _mockRepo = new Mock<IUser>();
            _userProfile = new UserProfile();
            _configuration = new MapperConfiguration(cfg => cfg.AddProfile(_userProfile));
            _mapper = new Mapper(_configuration);


            _registerUser.FirstName = "Django";
            _registerUser.Username = "django";
            _registerUser.LastName = "Jane";
            _registerUser.Password = "12345";

            _commonUser.FirstName = "Django";
            _commonUser.Username = "django";
            _commonUser.LastName = "Janet";
            _commonUser.Password = "12345";
        }
        public void Dispose()
        {
            _mockRepo = null;
            _configuration = null;
            _userProfile = null;
            _mapper = null;
        }


        [Fact]
        public async Task Login_Returns200OK_WhenUserIsAuthenticatedAsync()
        {
            //Arrange
            UserLogin Login_usr = _mapper.Map<UserLogin>(_commonUser);


            User usr = _commonUser;


            _mockRepo.Setup(repo =>
            repo.LoginAsync(It.IsAny<User>())).Returns(Task.FromResult(_commonUser));

            var controller = new UserController(_mockRepo.Object, _mapper);

            var result = await controller.UserLogin(Login_usr);
            var response = result as ResponsePayload;
            Assert.Equal(200, response.Code);

        }

        [Fact]
        public async Task Login_Returns404NotFound_WhenUserLoginFailsAsync()
        {
            //Arrange
            UserLogin Login_usr = _mapper.Map<UserLogin>(_commonUser);
            User usr = _commonUser;
            User user = _commonUser;

            _mockRepo.Setup(repo =>
            repo.LoginAsync(It.IsAny<User>())).Returns(Task.FromResult(user = null));

            var controller = new UserController(_mockRepo.Object, _mapper);

            var result = await controller.UserLogin(Login_usr);

            var response = result as ResponsePayload;

           Assert.Equal(404, response.Code);

        }

        [Fact]
        public async Task Register_Returns201OK_WhenUserIsSuccessfullyRegistered()
        {
            User usr = new();
            User user = _mapper.Map<User>(_registerUser);
            _mockRepo.Setup(c => c.RegisterAsync(It.IsAny<User>()))
                                    .Returns(Task.FromResult(user));

            _mockRepo.Setup(z => z.FindUserByUsernameAsync(It.IsAny<string>()))
                                    .Returns(Task.FromResult(usr));

            var controller = new UserController(_mockRepo.Object, _mapper);

            var result = await controller.UserRegister(_registerUser);
            var response = result as ResponsePayload;

            Assert.Equal(201, response.Code);
        }

        [Fact]
        public async Task Register_Returns400_WhenRegistrationFails()
        {

            User user = new();

            _mockRepo.Setup(c => c.RegisterAsync(It.IsAny<User>()))
                                            .Returns(Task.FromResult(user=null));


            var controller = new UserController(_mockRepo.Object, _mapper);

            var result = await controller.UserRegister(_registerUser);
            var response = result as ResponsePayload;
            Assert.Equal(400, response.Code);

        }

        [Fact]
        public async Task SearchUserByUsername_Returns_404NotFound_WhenNoHitsAreMade()
        {

            User user = new();

            _mockRepo.Setup(c => c.FindUserByUsernameAsync(It.IsAny<string>()))
                                            .Returns(Task.FromResult(user = null));


            var controller = new UserController(_mockRepo.Object, _mapper);

            var result = await controller.FindUserByUserName("string");
            var response = result as ResponsePayload;
            Assert.Equal(404, response.Code);

        }

        [Fact]
        public async Task SearchUserByUsername_Returns_200Ok_WhenHitsAreMade()
        {

            User user = new();

            _mockRepo.Setup(c => c.FindUserByUsernameAsync(It.IsAny<string>()))
                                            .Returns(Task.FromResult(_commonUser));


            var controller = new UserController(_mockRepo.Object, _mapper);

            var result = await controller.FindUserByUserName("string");
            var response = result as ResponsePayload;
            Assert.Equal(200, response.Code);

        }
    }
}
