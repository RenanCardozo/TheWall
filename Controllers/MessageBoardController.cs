using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using TheWall.Models;
using TheWall.Context;

namespace TheWall.Controllers;


public class MessageBoard : Controller
{
    private TheWallContext _context;

    public MessageBoard(TheWallContext context)
    {
        _context = context;
    }
    [Protected]
    [HttpGet("Messages")]
    public IActionResult Messageboard(MessageBoardView messageBoardView)
    {

        List<Message> messageList = _context.Messages
            .Include(m => m.Creator)
            .Include(m => m.Commenter)
                .ThenInclude(c => c.Creator)
            .OrderByDescending(m => m.CreatedAt)
            .ToList();

        MessageBoardView viewModel = new MessageBoardView { Messages = messageList };
        return View("Messageboard", viewModel);
    }

    [Protected]
    [HttpPost("AddMessage")]
    public IActionResult AddMessage(Message message)
    {
        if (message == null)
        {
            ModelState.AddModelError("MessageText", "Invalid message object.");
        }

        if (ModelState.IsValid)
        {
            int? userId = HttpContext.Session.GetInt32("userId");
            if (userId != null)
            {
                message.UserId = (int)userId;
                _context.Messages.Add(message);
                _context.SaveChanges();
                return RedirectToAction("Messageboard");
            }
        }
        List<Message> messages = _context.Messages
            .Include(m => m.Creator)
            .Include(m => m.Commenter)
                .ThenInclude(c => c.Creator)
            .OrderByDescending(m => m.CreatedAt)
            .ToList();

        MessageBoardView viewModel = new MessageBoardView { Messages = messages };
        return View("Messageboard", viewModel);
    }

    [Protected]
    [HttpPost("AddComment/{messageId}")]
    public IActionResult AddComment(int messageId, Comment comment)
    {
        Message? message = _context.Messages
            .Include(m => m.Commenter)
            .SingleOrDefault(m => m.MessageId == messageId);

        if (message != null && !string.IsNullOrEmpty(comment.CommentText))
        {
            Comment newComment = new Comment
            {
                CommentText = comment.CommentText,
                UserId = (int)HttpContext.Session.GetInt32("userId")
            };
            message.Commenter.Add(newComment);
            _context.SaveChanges();
        }

        return RedirectToAction("Messageboard");
    }

    [Protected]
    [HttpPost("DeleteMessage/{messageId}")]
    public IActionResult DeleteMessage(int messageId)
    {
        Message? message = _context.Messages.Find(messageId);
        if (message != null)
        {
            int? userId = HttpContext.Session.GetInt32("userId");
            if (userId != null && message.UserId == userId)
            {
                _context.Messages.Remove(message);
                _context.SaveChanges();
            }
        }

        return RedirectToAction("Messageboard");
    }

    [Protected]
    [HttpPost("DeleteComment/{commentId}")]
    public IActionResult DeleteComment(int commentId)
    {
        Comment? comment = _context.Comments.Find(commentId);
        if (comment != null)
        {
            int? userId = HttpContext.Session.GetInt32("userId");
            if (userId != null && comment.UserId == userId)
            {
                _context.Comments.Remove(comment);
                _context.SaveChanges();
            }
        }

        return RedirectToAction("Messageboard");
    }
    
}
public class ProtectedAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        int? _userId = context.HttpContext.Session.GetInt32("userId");
        if (_userId is null)
        {
            context.Result = new RedirectToActionResult("LoginRegister", "User", null);
        }
    }
}

