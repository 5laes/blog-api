using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using blogg_api.Data;
using blogg_api.Models;
using blogg_api.Models.DTOs;
using blogg_api.Services;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace blogg_api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithOrigins("http://localhost:3000")
                    .WithOrigins("http://localhost:5173"); // add azure url here
                });
            });

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("Connection")));

            builder.Services.AddScoped<IAppRepository<BlogTag>, BlogTagRepository>();
            builder.Services.AddValidatorsFromAssemblyContaining<BlogTagCreateDTO>();
            builder.Services.AddValidatorsFromAssemblyContaining<BlogTagUpdateDTO>();

            builder.Services.AddScoped<IAppRepository<BlogContent>, BlogContentRepository>();
            builder.Services.AddValidatorsFromAssemblyContaining<BlogContentCreateDTO>();
            builder.Services.AddValidatorsFromAssemblyContaining<BlogContentUpdateDTO>();

            builder.Services.AddScoped<IAppRepository<BlogPost>, BlogPostRepository>();
            builder.Services.AddScoped<IPostRepository<BlogPost>, BlogPostRepository>();
            builder.Services.AddScoped<IPostWithTagRepository<BlogPostWithTagsDTO>, BlogPostRepository>();
            builder.Services.AddValidatorsFromAssemblyContaining<BlogPostCreateDTO>();
            builder.Services.AddValidatorsFromAssemblyContaining<BlogPostUpdateDTO>();

            builder.Services.AddAutoMapper(typeof(Program).Assembly);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            // BlogTag Endpoints
            app.MapGet("/api/BlogTag", async (IAppRepository<BlogTag> repository) =>
            {
                ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = System.Net.HttpStatusCode.BadRequest };
                response.Result = await repository.GetAllAsync();

                if (response.Result == null)
                {
                    response.ErrorMessages.Add("Failed to get data from database");
                    return Results.BadRequest(response);
                }

                response.IsSuccess = true;
                response.StatusCode = System.Net.HttpStatusCode.OK;
                return Results.Ok(response);
            }).Produces<ApiResponse>(200).Produces(400);

            app.MapGet("/api/BlogTag/id", async (int id, IAppRepository<BlogTag> repository) =>
            {
                ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = System.Net.HttpStatusCode.BadRequest };
                response.Result = await repository.GetSingleAsync(id);

                if (response.Result == null)
                {
                    response.ErrorMessages.Add($"ERROR: No tag with id {id} exists");
                    return Results.BadRequest(response);
                }

                response.IsSuccess = true;
                response.StatusCode = System.Net.HttpStatusCode.OK;
                return Results.Ok(response);
            }).Produces<ApiResponse>(200).Produces(400);

            app.MapPost("/api/BlogTag",
            async (
            [FromServices] IValidator<BlogTagCreateDTO> validator,
            [FromServices] IMapper _mapper,
            [FromBody] BlogTagCreateDTO C_BlogTag_DTO,
            IAppRepository<BlogTag> repository) =>
            {
                ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = System.Net.HttpStatusCode.BadRequest };

                var validateInput = await validator.ValidateAsync(C_BlogTag_DTO);
                if (!validateInput.IsValid)
                {
                    foreach (var error in validateInput.Errors.ToList())
                    {
                        response.ErrorMessages.Add(error.ToString());
                    }
                    return Results.BadRequest(response);
                }

                BlogTag blogTag = _mapper.Map<BlogTag>(C_BlogTag_DTO);

                response.Result = await repository.AddAsync(blogTag);

                if (response.Result == null)
                {
                    response.ErrorMessages.Add($"ERROR: A Tag with the name '{blogTag.TagName}' already exists");
                    response.StatusCode = System.Net.HttpStatusCode.Conflict;
                    return Results.Conflict(response);
                }

                response.Result = _mapper.Map<BlogTagCreateDTO>(blogTag);
                response.IsSuccess = true;
                response.StatusCode = System.Net.HttpStatusCode.OK;

                return Results.Ok(response);
            }).Accepts<BlogTagCreateDTO>("application/json").Produces<ApiResponse>(201).Produces(400).Produces(409);

            app.MapDelete("/api/BlogTag/{id:int}", async (int id, IAppRepository<BlogTag> repository) =>
            {
                ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = System.Net.HttpStatusCode.BadRequest };

                response.Result = await repository.DeleteAsync(id);
                if (response.Result == null)
                {
                    response.ErrorMessages.Add($"ERROR: No Tag with id {id} exists");
                    return Results.BadRequest(response);
                }
                response.IsSuccess = true;
                response.StatusCode = System.Net.HttpStatusCode.NoContent;
                return Results.Ok(response);
            }).Produces<ApiResponse>(204).Produces(400);

            app.MapPut("/api/BlogTag",
            async (
            [FromServices] IValidator<BlogTagUpdateDTO> validator,
            [FromServices] IMapper _mapper,
            [FromBody] BlogTagUpdateDTO U_BlogTag_DTO,
            IAppRepository<BlogTag> repository) =>
            {
                ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = System.Net.HttpStatusCode.BadRequest };

                var validateInput = await validator.ValidateAsync(U_BlogTag_DTO);
                if (!validateInput.IsValid)
                {
                    foreach (var error in validateInput.Errors.ToList())
                    {
                        response.ErrorMessages.Add(error.ToString());
                    }
                    return Results.BadRequest(response);
                }

                BlogTag blogTag = _mapper.Map<BlogTag>(U_BlogTag_DTO);

                response.Result = await repository.UpdateAsync(blogTag);

                if (response.Result == null)
                {
                    response.ErrorMessages.Add($"ERROR: No Tag with id {blogTag.Id} exists");
                    return Results.BadRequest(response);
                }

                response.Result = _mapper.Map<BlogTagUpdateDTO>(blogTag);
                response.IsSuccess = true;
                response.StatusCode = System.Net.HttpStatusCode.OK;

                return Results.Ok(response);
            }).Accepts<BlogTagUpdateDTO>("application/json").Produces<ApiResponse>(200).Produces(400);


            // BlogContent Endpoints

            app.MapGet("/api/BlogContent", async (IAppRepository<BlogContent> repository) =>
            {
                ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = System.Net.HttpStatusCode.BadRequest };
                response.Result = await repository.GetAllAsync();

                if (response.Result == null)
                {
                    response.ErrorMessages.Add("Failed to get data from database");
                    return Results.BadRequest(response);
                }

                response.IsSuccess = true;
                response.StatusCode = System.Net.HttpStatusCode.OK;
                return Results.Ok(response);
            }).Produces<ApiResponse>(200).Produces(400);

            app.MapGet("/api/BlogContent/id", async (int id, IAppRepository<BlogContent> repository) =>
            {
                ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = System.Net.HttpStatusCode.BadRequest };
                response.Result = await repository.GetSingleAsync(id);
                if (response.Result == null)
                {
                    return Results.BadRequest(response);
                }
                response.IsSuccess = true;
                response.StatusCode = System.Net.HttpStatusCode.OK;
                return Results.Ok(response);
            }).Produces<ApiResponse>(200).Produces(400);

            app.MapPost("/api/BlogContent",
            async (
            [FromServices] IValidator<BlogContentCreateDTO> validator,
            [FromServices] IMapper _mapper,
            [FromBody] BlogContentCreateDTO C_BlogContent_DTO,
            IAppRepository<BlogContent> repository) =>
            {
                ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = System.Net.HttpStatusCode.BadRequest };

                var validateInput = await validator.ValidateAsync(C_BlogContent_DTO);
                if (!validateInput.IsValid)
                {
                    foreach (var error in validateInput.Errors.ToList())
                    {
                        response.ErrorMessages.Add(error.ToString());
                    }
                    return Results.BadRequest(response);
                }

                BlogContent content = _mapper.Map<BlogContent>(C_BlogContent_DTO);
                content.DatePublished = DateTime.Now;

                response.Result = await repository.AddAsync(content);

                if (response.Result == null)
                {
                    response.ErrorMessages.Add("ERROR: Something went wrong");
                    response.StatusCode = System.Net.HttpStatusCode.Conflict;
                    return Results.Conflict(response);
                }

                response.Result = _mapper.Map<BlogContentCreateDTO>(content); 
                response.IsSuccess = true;
                response.StatusCode = System.Net.HttpStatusCode.OK;
                return Results.Ok(response);
            }).Accepts<BlogContentCreateDTO>("application/json").Produces<ApiResponse>(201).Produces(400).Produces(409);

            app.MapDelete("/api/BlogContent/{id:int}", async (int id, IAppRepository<BlogContent> repository) =>
            {
                ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = System.Net.HttpStatusCode.BadRequest };

                response.Result = await repository.DeleteAsync(id);

                if (response.Result == null)
                {
                    response.ErrorMessages.Add($"No content with id {id} exists");
                    return Results.BadRequest(response);
                }

                response.IsSuccess = true;
                response.StatusCode = System.Net.HttpStatusCode.OK;
                return Results.Ok(response);
            }).Produces<ApiResponse>(200).Produces(400);

            app.MapPut("/api/BlogContent",
            async (
            [FromServices] IValidator<BlogContentUpdateDTO> validator,
            [FromServices] IMapper _mapper,
            [FromBody] BlogContentUpdateDTO U_BlogContent_DTO,
            IAppRepository<BlogContent> repository) =>
            {
                ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = System.Net.HttpStatusCode.BadRequest };

                var validateInput = await validator.ValidateAsync(U_BlogContent_DTO);
                if (!validateInput.IsValid)
                {
                    foreach (var error in validateInput.Errors.ToList())
                    {
                        response.ErrorMessages.Add(error.ToString());
                    }
                    return Results.BadRequest(response);
                }

                BlogContent content = _mapper.Map<BlogContent>(U_BlogContent_DTO);

                response.Result = await repository.UpdateAsync(content);

                if (response.Result == null)
                {
                    response.ErrorMessages.Add($"ERROR: No content with id");
                    return Results.BadRequest(response);
                }

                response.Result = _mapper.Map<BlogContentUpdateDTO>(content);
                response.IsSuccess = true;
                response.StatusCode = System.Net.HttpStatusCode.OK;
                return Results.Ok(response);
            }).Produces<ApiResponse>(200).Produces(400);




            // BlogPost Endpoints
            app.MapGet("/api/BlogPost", async (IAppRepository<BlogPost> repository) =>
            {
                ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = System.Net.HttpStatusCode.BadRequest };
                response.Result = await repository.GetAllAsync();

                if (response.Result == null)
                {
                    response.ErrorMessages.Add("Failed to get data from database");
                    return Results.BadRequest(response);
                }

                response.IsSuccess = true;
                response.StatusCode = System.Net.HttpStatusCode.OK;
                return Results.Ok(response);
            }).Produces<ApiResponse>(200).Produces(400);

            app.MapGet("/api/BlogPostsWithTags", async (IPostWithTagRepository<BlogPostWithTagsDTO> repository) =>
            {
                ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = System.Net.HttpStatusCode.BadRequest };
                response.Result = await repository.GetPostsWithTagsAsync();

                if (response.Result == null)
                {
                    response.ErrorMessages.Add("Failed to get data from database");
                    return Results.BadRequest(response);
                }

                response.IsSuccess = true;
                response.StatusCode = System.Net.HttpStatusCode.OK;
                return Results.Ok(response);
            }).Produces<ApiResponse>(200).Produces(400);

            app.MapGet("api/BlogPost/{id:int}", async (IPostRepository<BlogPost> repository, int Id) =>
            {
                ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = System.Net.HttpStatusCode.BadRequest };
                response.Result = await repository.GetPostWithTagsAsync(Id);

                if (response.Result == null)
                {
                    response.ErrorMessages.Add($"No post with id {Id} found");
                    return Results.BadRequest(response);
                }

                response.IsSuccess = true;
                response.StatusCode = System.Net.HttpStatusCode.OK;
                return Results.Ok(response);
            }).Produces<ApiResponse>(200).Produces(400);

            app.MapGet("api/BlogPostTag/{id:int}", async (IPostWithTagRepository<BlogPostWithTagsDTO> repository, int Id) =>
            {
                ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = System.Net.HttpStatusCode.BadRequest };
                response.Result = await repository.GetPostsByTagAsync(Id);

                if (response.Result == null)
                {
                    response.ErrorMessages.Add($"No post with tag id {Id} found");
                    return Results.BadRequest(response);
                }

                response.IsSuccess = true;
                response.StatusCode = System.Net.HttpStatusCode.OK;
                return Results.Ok(response);
            }).Produces<ApiResponse>(200).Produces(400);

            app.MapPost("/api/BlogPost",
            async (
            [FromServices] IValidator<BlogPostCreateDTO> validator,
            [FromServices] IMapper _mapper,
            [FromBody] BlogPostCreateDTO C_BlogPost_DTO,
            IAppRepository<BlogPost> repository) =>
            {
                ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = System.Net.HttpStatusCode.BadRequest };

                var validateInput = await validator.ValidateAsync(C_BlogPost_DTO);
                if (!validateInput.IsValid)
                {
                    foreach (var error in validateInput.Errors.ToList())
                    {
                        response.ErrorMessages.Add(error.ToString());
                    }
                    return Results.BadRequest(response);
                }

                BlogPost content = _mapper.Map<BlogPost>(C_BlogPost_DTO);

                response.Result = await repository.AddAsync(content);

                if (response.Result == null)
                {
                    response.ErrorMessages.Add("ERROR: Something went wrong");
                    response.StatusCode = System.Net.HttpStatusCode.Conflict;
                    return Results.Conflict(response);
                }

                response.Result = _mapper.Map<BlogPostCreateDTO>(content);
                response.IsSuccess = true;
                response.StatusCode = System.Net.HttpStatusCode.OK;
                return Results.Ok(response);
            }).Accepts<BlogPostCreateDTO>("application/json").Produces<ApiResponse>(201).Produces(400).Produces(409);

            app.MapDelete("/api/BlogPost", async (IAppRepository<BlogPost> repository, int Id) =>
            {
                ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = System.Net.HttpStatusCode.BadRequest };

                response.Result = await repository.DeleteAsync(Id);

                if (response.Result == null)
                {
                    response.ErrorMessages.Add($"No post with id {Id} exists");
                    return Results.BadRequest(response);
                }

                response.IsSuccess = true;
                response.StatusCode = System.Net.HttpStatusCode.OK;
                return Results.Ok(response);
            }).Produces<ApiResponse>(200).Produces(400);

            app.MapDelete("/api/BlogPostTag", async (IPostRepository<BlogPost> repository, int Id, int tId) =>
            {
                ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = System.Net.HttpStatusCode.BadRequest };

                response.Result = await repository.RemoveTagAsync(Id, tId);

                if (response.Result == null)
                {
                    response.ErrorMessages.Add($"No post with id {Id} and tag id {tId}");
                    return Results.BadRequest(response);
                }

                response.IsSuccess = true;
                response.StatusCode = System.Net.HttpStatusCode.OK;
                return Results.Ok(response);
            }).Produces<ApiResponse>(200).Produces(400);



            app.Run();
        }
    }
}