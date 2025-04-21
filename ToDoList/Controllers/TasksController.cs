using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.DTOs;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    [Route("tasks")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ToDoContext _context;
        public TasksController(ToDoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks()
        {
            return await _context.TaskItems.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> GetTask(int id)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task == null)
                return NotFound();
            return  Ok(task);
        }
        [HttpPost]
        public async Task<ActionResult<TaskItem>> CreateTask(TaskCreateUpdateDto dto)
        {
            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                IsCompleted = dto.IsCompleted,
            };
            _context.TaskItems.Add(task);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTask(int id, TaskCreateUpdateDto dto)
        {
            var Task = await _context.TaskItems.FindAsync(id);
            if (Task == null)
                return NotFound();

            Task.Title = dto.Title;
            Task.Description = dto.Description;
            Task.IsCompleted = dto.IsCompleted;

            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<TaskItem>> DeleteProduct(int id)
        {
            var items = await _context.TaskItems.FindAsync(id);
            if (items == null)
            {
                return NotFound();
            }

            _context.TaskItems.Remove(items);
            await _context.SaveChangesAsync();

            return items;
        }

    }
}
