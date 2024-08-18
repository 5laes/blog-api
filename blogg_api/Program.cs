using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using blogg_api.Data;
using blogg_api.Models;
using blogg_api.Models.DTOs;
using blogg_api.Services;
using System.ComponentModel.DataAnnotations;

namespace blogg_api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

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

            builder.Services.AddAutoMapper(typeof(Program).Assembly);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapGet("/api/BlogTag", async (IAppRepository<BlogTag> repository) =>
            {
                ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = System.Net.HttpStatusCode.BadRequest };
                response.Result = await repository.GetAllAsync();
                response.IsSuccess = true;
                response.StatusCode = System.Net.HttpStatusCode.OK;
                return Results.Ok(response);
            }).Produces<ApiResponse>(200);

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
                ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = System.Net.HttpStatusCode.NotFound };

                response.Result = await repository.DeleteAsync(id);
                if (response.Result == null)
                {
                    response.ErrorMessages.Add($"ERROR: No Tag with id {id} exists");
                    return Results.NotFound(response);
                }
                response.IsSuccess = true;
                response.StatusCode = System.Net.HttpStatusCode.NoContent;
                return Results.Ok(response);
            }).Produces<ApiResponse>(204).Produces(404);

            app.MapPut("/api/BlogTag",
            async (
            [FromServices] IValidator<BlogTagUpdateDTO> validator,
            [FromServices] IMapper _mapper,
            [FromBody] BlogTagUpdateDTO U_BlogTag_DTO,
            IAppRepository<BlogTag> repository) =>
            {
                ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = System.Net.HttpStatusCode.NotFound };

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

            app.Run();
        }
    }
}