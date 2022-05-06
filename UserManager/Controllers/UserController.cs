using Application.DTO.User;
using Application.Response;
using AutoMapper;
using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace UserManager.Controllers
{   
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUser _service;
        private ResponsePayload _response;
        private readonly IMapper _mapper;

        public UserController(IUser service,IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
            _response = new ResponsePayload();
        }

        [HttpGet]
        public async Task<object> Index()
        {
            var results = await _service.RetrieveAllUsersAsync();
            _response.Data = _mapper.Map<IEnumerable<UserView>>(results);
            _response.Code = Ok().StatusCode;
            return _response;
        }

        [HttpPost("Login")]
        public async Task<object> UserLogin([FromBody] UserLogin userLogin)
        {
            try
            {
                User usr = _mapper.Map<User>(userLogin);
                var success = await _service.LoginAsync(usr);
                if (success!=null)
                {
                    _response.Data = _mapper.Map<UserFull>(success);
                    _response.Code = Ok().StatusCode;
                }
                else
                {
                    _response.Code = NotFound().StatusCode;
                    _response.Message = "Login failed";
                }
            }
            catch (Exception ex)
            {

                _response.ErrorMessages = new List<string>{ex.Message.ToString()};
                _response.Code = 500;
                _response.IsSuccessful = false;
            }
            return _response;
        }

        [HttpPost("Register")]
        public async Task<object> UserRegister([FromBody] UserAdd newUser)
        {
            try
            {
                User usr = _mapper.Map<User>(newUser);
                var success = await _service.RegisterAsync(usr);
                if (success != null)
                {
                    _response.Data = _mapper.Map<UserView>(success);
                    _response.Code = 201;
                }
                else
                {
                    _response.Code = BadRequest().StatusCode;
                    _response.Message = "Registration failed";
                }
            }
            catch (Exception ex)
            {

                _response.ErrorMessages = new List<string> { ex.Message.ToString() };
                _response.Code = 500;
                _response.IsSuccessful = false;
            }
            return _response;
        }

        [HttpGet("By-Name/{name}")]
        public async Task<object> FindUserByName(string name)
        {
            try
            {
                var results = await _service.FindUserByNameAsync(name);
                if (results.Any())
                {
                    _response.Data = _mapper.Map<IEnumerable<UserView>>(results);
                    _response.Code = Ok().StatusCode;
                }
                else
                {
                    _response.Code = BadRequest().StatusCode;
                    _response.Message = "User search failed";
                }
            }
            catch (Exception ex)
            {

                _response.ErrorMessages = new List<string> { ex.Message.ToString() };
                _response.Code = 500;
                _response.IsSuccessful = false;
            }
            return _response;
        }

        [HttpGet("By-Username/{username}")]
        public async Task<object> FindUserByUserName(string username)
        {
            try
            {
                var user = await _service.FindUserByUsernameAsync(username);
                if (user != null)
                {
                    _response.Data = _mapper.Map<UserFull>(user);
                    _response.Code = Ok().StatusCode;
                }
                else
                {
                    _response.Code = NotFound().StatusCode;
                    _response.Message = "User search failed";
                }
            }
            catch (Exception ex)
            {

                _response.ErrorMessages = new List<string> { ex.Message.ToString() };
                _response.Code = 500;
                _response.IsSuccessful = false;
            }
            return _response;
        }

        [HttpPatch("Update/{username}")]
        public async Task<object> PartiallyUpdateUser(string username, JsonPatchDocument<UserUpdate> userUpdate)
        {
            try
            {
                var modelFromRepo = await _service.FindUserByUsernameAsync(username);

                if (modelFromRepo == null)
                {
                    _response.Code = NotFound().StatusCode;
                    _response.Message = "User search failed";
                }
                var userToUpdate = _mapper.Map<UserUpdate>(modelFromRepo);
                userUpdate.ApplyTo(userToUpdate, ModelState);

                if (!TryValidateModel(userToUpdate))
                {
                    return ValidationProblem(ModelState);
                }
                _mapper.Map(userToUpdate, modelFromRepo);

                _service.UpdateInformation(modelFromRepo);
                _service.SaveChanges();
                var obj = await _service.FindUserByUsernameAsync(username);
                _response.Data = _mapper.Map<UserFull>(obj);
                _response.Code = Ok().StatusCode;

            }
            catch (Exception ex)
            {

                _response.ErrorMessages = new List<string> { ex.Message.ToString() };
                _response.Code = 500;
                _response.IsSuccessful = false;
            }
            return _response;
        }
    }
}
