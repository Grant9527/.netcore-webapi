using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TodoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace TodoApi.Controllers
{
  [ApiController]
  [Route("api/[controller]/[action]")]
  public class UsersController : ControllerBase
  {
    private readonly TodoContext _context;

    public UsersController(TodoContext context)
    {
      _context = context;
    }

    private static string StrKeyWord = "select|insert|delete|from|count(|drop table|update|truncate|asc(|mid(|char(|xp_cmdshell|exec|master|net local group administrators|net user|or|and";
    private static string StrSymbol = ";|(|)|[|]|{|}|%|@|*|'|!";

    //检验参数是否有SQL语句注入
    private static bool CheckKeyWord(string _key)
    {
      string[] pattenKeyWord = StrKeyWord.Split('|');
      string[] pattenSymbol = StrSymbol.Split('|');
      foreach (string sqlParam in pattenKeyWord)
      {
        if (_key.Contains(sqlParam + " ") || _key.Contains(" " + sqlParam))
        {
          return true;
        }
      }
      foreach (string sqlParam in pattenSymbol)
      {
        if (_key.Contains(sqlParam))
        {
          return true;
        }
      }
      return false;
    }

    [HttpGet]
    public async Task<ActionResult<Users>> GetDataById(int id)
    {
      var todoItem = await _context.Users.SingleOrDefaultAsync(i => i.id == id);
      if (todoItem == null)
      {
        return NotFound();
      }
      return todoItem;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Users>>> GetAll(int id, string username, string password)
    {
      //return await _context.TodoItems.ToListAsync();

      string whereStr = "";
      if (!string.IsNullOrEmpty(id.ToString()) && id != 0)
      {
        if (CheckKeyWord(id.ToString()))
        {
          return BadRequest();
        }
        whereStr += " and id= " + id;
      }
      if (!string.IsNullOrEmpty(username))
      {
        if (CheckKeyWord(username))
        {
          return BadRequest();
        }
        whereStr += " and username like '%" + username + "%'";
      }
      if (!string.IsNullOrEmpty(password))
      {
        if (CheckKeyWord(password))
        {
          return BadRequest();
        }
        whereStr += " and password= " + password;
      }

      string sql = "select * from dbo.Users where 1=1 " + whereStr;
      return await _context.Users
          .FromSqlRaw(sql)
          .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Users>> AddData(Users item)
    {
      _context.Users.Add(item);
      var i = await _context.SaveChangesAsync();
      if (i > 0)
      {
        return CreatedAtAction(nameof(GetDataById), new { id = item.id }, item);
      }
      else
      {
        return null;
      }

    }

    [HttpPut("{id}")]
    public async Task<IActionResult> updateData(int id, Users item)
    {
      if (id != item.id)
      {
        return BadRequest();
      }
      _context.Entry(item).State = EntityState.Modified;
      await _context.SaveChangesAsync();
      return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> deleteData(int id)
    {
      var todoItem = await _context.Users.FindAsync(id);
      if (todoItem == null)
      {
        return NotFound();
      }
      _context.Users.Remove(todoItem);
      await _context.SaveChangesAsync();
      return NoContent();
    }


  }
}