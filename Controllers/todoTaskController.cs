using Microsoft.AspNetCore.Mvc;

namespace myAPI.Controllers

{
    public class todoTaskController : ControllerBase
    {
        public static List<Task> Tasks = new List<Task>();
        public static List<TodoAssignee> Assignees = new List<TodoAssignee>();

        [HttpPost("/api/tasks")]
        public IActionResult Create([FromBody] TaskDto taskDto)
        {
            try
            {
                if (taskDto == null)
                {
                    return BadRequest("Task details are required.");
                }

                var newTask = new Task
                {
                    Id = Tasks.Count + 1,
                    Title = taskDto.Title,
                    Description = taskDto.Description,
                    Status = TaskStatus.complete
                };

                Tasks.Add(newTask);
                return CreatedAtAction(nameof(GetById), new { id = newTask.Id }, newTask);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("/api/tasks")]
        public IActionResult GetAll()
        {
            try
            {
                var taskList = Tasks.Select(task => new
                {
                    task.Id,
                    task.Title,
                    task.Description,
                    task.Status,
                    AssigneeName = Assignees.FirstOrDefault(a => a.TodoId == task.Id)?.PersonName
                }).ToList();

                return Ok(taskList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("/api/tasks/{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var task = Tasks.FirstOrDefault(t => t.Id == id);

                if (task == null)
                {
                    return NotFound($"Task with ID {id} not found.");
                }

                var assignee = Assignees.FirstOrDefault(a => a.TodoId == id);
                return Ok(new
                {
                    task.Id,
                    task.Title,
                    task.Description,
                    task.Status,
                    AssigneeName = assignee?.PersonName
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("/api/tasks/{id}")]
        public IActionResult Update(int id, [FromBody] TaskDto taskDto)
        {
            try
            {
                var task = Tasks.FirstOrDefault(t => t.Id == id);

                if (task == null)
                {
                    return NotFound($"Task with ID {id} not found.");
                }

                task.Title = taskDto.Title;
                task.Description = taskDto.Description;
                task.Status = taskDto.Status;

                return Ok("Task updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("/api/tasks/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var task = Tasks.FirstOrDefault(t => t.Id == id);

                if (task == null)
                {
                    return NotFound($"Task with ID {id} not found.");
                }

                Tasks.Remove(task);
                Assignees.RemoveAll(a => a.TodoId == id);

                return Ok("Task deleted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("/api/tasks/assign")]
        public IActionResult AssignTask([FromBody] TodoAssigneeDto assigneeDto)
        {
            try
            {
                if (assigneeDto == null)
                {
                    return BadRequest("Assignee details are required.");
                }

                var task = Tasks.FirstOrDefault(t => t.Id == assigneeDto.TodoId);

                if (task == null)
                {
                    return NotFound($"Task with ID {assigneeDto.TodoId} not found.");
                }

                Assignees.RemoveAll(a => a.TodoId == assigneeDto.TodoId);

                Assignees.Add(new TodoAssignee
                {
                    TodoId = assigneeDto.TodoId,
                    PersonId = assigneeDto.PersonId,
                    PersonName = assigneeDto.PersonName
                });

                return Ok("Task assigned successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

public class TaskDto
{
    public string Title { get; set; }
    public string Description { get; set; }

    public TaskStatus Status { get; set; }
}

public class Task
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }

    public TaskStatus Status { get; set; }
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

public enum TaskStatus
{
    complete,
    pending,
    inprogress,
}