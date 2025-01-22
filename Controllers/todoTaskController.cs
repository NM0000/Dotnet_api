using Microsoft.AspNetCore.Mvc;

namespace myAPI.Controllers

{
    
    [ApiController]
    public class todotodoTaskController : ControllerBase
    {
        public static List<Task> Tasks = new List<Task>();
        public static List<TodoAssignee> Assignees = new List<TodoAssignee>();

        [HttpPost("/api/tasks")]
        public IActionResult Create([FromBody] TaskDto taskDto)
        {
            var task = new Task
            {
                Id = Tasks.Count + 1,
                Title = taskDto.Title,
                Description = taskDto.Description,
                IsCompleted = taskDto.IsCompleted
            };

            Tasks.Add(task);
            return Ok(task);
        }

        [HttpGet("/api/tasks")]
        public IActionResult GetAll()
        {
            var tasksWithAssignees = Tasks.Select(task =>
            {
                var assignee = Assignees.FirstOrDefault(a => a.TodoId == task.Id);
                return new
                {
                    TaskId = task.Id,
                    TaskTitle = task.Title,
                    TaskDescription = task.Description,
                    IsCompleted = task.IsCompleted,
                    AssigneeName = assignee?.PersonName
                };
            }).ToList();

            return Ok(tasksWithAssignees);
        }

        [HttpGet("/api/tasks/{id}")]
        public IActionResult GetById(int id)
        {
            var task = Tasks.FirstOrDefault(x => x.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            var assignee = Assignees.FirstOrDefault(a => a.TodoId == id);
            var taskWithAssignee = new
            {
                TaskId = task.Id,
                TaskTitle = task.Title,
                TaskDescription = task.Description,
                IsCompleted = task.IsCompleted,
                AssigneeName = assignee?.PersonName
            };

            return Ok(taskWithAssignee);
        }

        [HttpPut("/api/tasks/{id}")]
        public IActionResult Update(int id, [FromBody] TaskDto taskDto)
        {
            var existingTask = Tasks.FirstOrDefault(x => x.Id == id);

            if (existingTask == null)
            {
                return NotFound();
            }

            existingTask.Title = taskDto.Title;
            existingTask.Description = taskDto.Description;
            existingTask.IsCompleted = taskDto.IsCompleted;

            return Ok("Task updated successfully");
        }

        [HttpDelete("/api/tasks/{id}")]
        public IActionResult Delete(int id)
        {
            var task = Tasks.FirstOrDefault(x => x.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            Tasks.Remove(task);
            Assignees.RemoveAll(a => a.TodoId == id); // Remove related assignees
            return Ok("Task deleted successfully");
        }

        [HttpPost("/api/tasks/assign")]
        public IActionResult AssignTask([FromBody] TodoAssigneeDto assigneeDto)
        {
            var task = Tasks.FirstOrDefault(x => x.Id == assigneeDto.TodoId);
            if (task == null)
            {
                return NotFound("Task not found.");
            }

            var assignee = new TodoAssignee
            {
                TodoId = assigneeDto.TodoId,
                TodoName = task.Title,
                PersonId = assigneeDto.PersonId,
                PersonName = assigneeDto.PersonName
            };

            Assignees.RemoveAll(a => a.TodoId == assigneeDto.TodoId); // Ensure only one assignee per task
            Assignees.Add(assignee);

            return Ok("Task assigned successfully");
        }
    }

    public class TaskDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
    }

    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
    }

    public class TodoAssigneeDto
    {
        public int TodoId { get; set; }
        public string PersonId { get; set; }
        public string PersonName { get; set; }
    }

    public class TodoAssignee
    {
        public int TodoId { get; set; }
        public string TodoName { get; set; }
        public string PersonId { get; set; }
        public string PersonName { get; set; }
    }
}