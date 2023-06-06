using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using Gaos.Dbo;
using gaos.Dbo.Model;

namespace Gaos.Routes
{
    public static class TodoItemsRoutes
    {
        public static RouteGroupBuilder GroupTodosItems(this RouteGroupBuilder group)
        {
            group.MapGet("/hello", (Db db) => "Hello!");

            group.MapGet("/", async (Db db) =>
                await db.Todos.ToListAsync());

            group.MapGet("/complete", async (Db db) =>
                await db.Todos.Where(t => t.IsComplete).ToListAsync());

            group.MapGet("/{id}", async (int id, Db db) =>
                await db.Todos.FindAsync(id)
                    is Todo todo
                        ? Results.Ok(todo)
                        : Results.NotFound());

            group.MapPost("/", async (Todo todo, Db db) =>
            {
                db.Todos.Add(todo);
                await db.SaveChangesAsync();

                return Results.Created($"/todoitems/{todo.Id}", todo);
            });

            group.MapPut("/{id}", async (int id, Todo inputTodo, Db db) =>
            {
                var todo = await db.Todos.FindAsync(id);

                if (todo is null) return Results.NotFound();

                todo.Name = inputTodo.Name;
                todo.IsComplete = inputTodo.IsComplete;

                await db.SaveChangesAsync();

                return Results.NoContent();
            });

            group.MapDelete("/{id}", async (int id, Db db) =>
            {
                if (await db.Todos.FindAsync(id) is Todo todo)
                {
                    db.Todos.Remove(todo);
                    await db.SaveChangesAsync();
                    return Results.Ok(todo);
                }

                return Results.NotFound();
            });
            return group;
        }
    }
}
