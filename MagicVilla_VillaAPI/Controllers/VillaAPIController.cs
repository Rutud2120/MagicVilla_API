using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.DTO;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {

        [HttpGet]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {

            return Ok(VillaStore.villaList);

        }


        [HttpGet("id", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<VillaDTO> GetVilla(int Id)
        {
            if (Id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == Id);

            if (villa == null)
            {
                return NotFound();
            }

            return Ok(villa);


        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<VillaDTO> CreateVilla([FromBody] VillaDTO villa)
        {
            //if(!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            if (VillaStore.villaList.FirstOrDefault(u => u.Name.ToLower() == villa.Name.ToLower()) != null)
            {
                ModelState.AddModelError("Custom Error", "Name Already Exists");
                return BadRequest(ModelState);
            }

            if (villa == null)
            {
                return BadRequest(villa);
            }
            if (villa.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            villa.Id = VillaStore.villaList.OrderByDescending(u => u.Id).First().Id + 1;


            VillaStore.villaList.Add(villa);
            return CreatedAtRoute("GetVilla", new { id = villa.Id }, villa);


        }

        [HttpDelete("{Id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public IActionResult DeleteVilla(int Id)
        {
            if (Id == 0)
            {
                return BadRequest();

            }

            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == Id);

            if (villa == null)
            {
                return NotFound();
            }

            VillaStore.villaList.Remove(villa);

            return NoContent();


        }


        [HttpPut("{Id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public IActionResult UpdateVilla(int Id, [FromBody] VillaDTO villa)
        {
            if (villa == null || Id != villa.Id)
            {
                return BadRequest();
            }

            var upvilla = VillaStore.villaList.FirstOrDefault(u => u.Id == Id);
            upvilla.Name = villa.Name;
            upvilla.Occupancy = villa.Occupancy;
            upvilla.Sqft = villa.Sqft;

            return NoContent();
        }

        /*
        The key point here is that the upvilla variable is a reference to the original villa object within VillaStore.villaList. When you modify the properties of upvilla, you are effectively modifying the properties of the original object within the list. In C#, classes are reference types, and when you assign one instance of a class to another, they both reference the same object in memory.

So, when you update upvilla.Name, upvilla.Occupancy, and upvilla.Sqft with the values from the villa object, you are updating the properties of the original villa object within VillaStore.villaList. Here's a breakdown:

upvilla is a reference to a specific villa object in VillaStore.villaList.
When you use upvilla.Name = villa.Name;, you are updating the Name property of the original villa object in the list.
Similarly, upvilla.Occupancy = villa.Occupancy; and upvilla.Sqft = villa.Sqft; update the Occupancy and Sqft properties of the original villa object.
Therefore, the changes made to upvilla are reflected in the original object within the list (VillaStore.villaList), and this is how the original data in VillaStore.villaList is updated.
        */



        [HttpPatch("{Id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public IActionResult UpdatePartialVilla(int Id, JsonPatchDocument <VillaDTO>   parVilla)
        {
            if (parVilla == null || Id==0)
            {
                return BadRequest();
            }

            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == Id);

            if(villa == null)
            {
                return NotFound();
            }


            parVilla.ApplyTo(villa, ModelState);

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);       
                /*returns a BadRequest (HTTP 400) response along with the ModelState. Returning 
                 * the ModelState in the response body can be useful for client-side validation, as 
                 * it provides information about what validation errors occurred and 
                 * why the request is considered invalid.*/
            }


            return NoContent();
        }

    }
}

