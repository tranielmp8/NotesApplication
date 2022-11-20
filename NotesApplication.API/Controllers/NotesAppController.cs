using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesApplication.API.Data;
using NotesApplication.API.Models.Entities;

namespace NotesApplication.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")] //can do [Route("api/notesapp")]
    public class NotesAppController : Controller
    {
        private readonly NotesAppDbContext notesAppDbContext;

        public NotesAppController(NotesAppDbContext notesAppDbContext)
        {
            this.notesAppDbContext = notesAppDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNotes()
        {
            //Get the notes from Database
            return Ok(await notesAppDbContext.NotesApp.ToListAsync());
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [ActionName("GetNoteById")]
        public async Task<IActionResult> GetNoteById([FromRoute] Guid id) //[FromRoute] not really required, but it helps us know where the id matc
        {
            //await notesAppDbContext.NotesApp.FirstOrDefaultAsync(x => x.Id == id);
            // or
            var note = await notesAppDbContext.NotesApp.FindAsync(id);
            if(note == null)
            {
                return NotFound();
            }

            return Ok(note);
        }

        [HttpPost]
        public async Task<IActionResult> AddNote(Note note)
        {
            note.Id = Guid.NewGuid();
            await notesAppDbContext.NotesApp.AddAsync(note);
            await notesAppDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNoteById), new { id = note.Id }, note);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateNote([FromRoute] Guid id, [FromBody] Note updateNote)
        {
            var existingNote = await notesAppDbContext.NotesApp.FindAsync(id);

            if(existingNote == null)
            {
                return NotFound();
            }

            existingNote.Title = updateNote.Title;
            existingNote.Description = updateNote.Description;
            existingNote.IsVisible= updateNote.IsVisible;

            notesAppDbContext.SaveChanges();

            return Ok(existingNote);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteNote([FromRoute] Guid id)
        {
            var existingNote = await notesAppDbContext.NotesApp.FindAsync(id);
            if(existingNote== null)
            {
                return NotFound();
            }

            notesAppDbContext.NotesApp.Remove(existingNote);
            await notesAppDbContext.SaveChangesAsync();

            return Ok();
        }


    }
}
