using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net.Http.Formatting;
using parrot.Models;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;

namespace parrot.Services
{
    
    public static class PetService
    {
        public static void Init(){
            client.BaseAddress = new Uri("https://hieu.azure-api.net/petronas/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }
        static HttpClient client = new HttpClient();


        public static void ShowProduct(Pet pet)
        {
            Console.WriteLine($"Name: {pet.name}\tType: " +
                $"{pet.animal_type}\tId: {pet.id}");
        }

        public static async Task<bool> CreatePetAsync(string petId, Pet pet)
        {
            if(pet.tags == null) pet.tags = new Tags();
            pet.created = DateTime.Now;
            var s = JsonConvert.SerializeObject(pet);
            HttpResponseMessage response = await client.PutAsync( 
                $"pets/{petId}", new StringContent(s,Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.
            return response.IsSuccessStatusCode;
        }

        public static async Task<Pet> GetPetAsync(string petId)
        {
            Pet pet = null;
            HttpResponseMessage response = await client.GetAsync("pets/"+petId);
            if (response.IsSuccessStatusCode)
            {
                pet = await response.Content.ReadAsAsync<Pet>();
            }
            return pet;
        }

        public static async Task<PetResponse> GetPetsAsync(string animal_type = null)
        {
            PetResponse pets = null;
            var animalTypeStr = (animal_type == null) ? "" : ("animal_type="+animal_type+"&"); 
            HttpResponseMessage response = await client.GetAsync("pets?" + animalTypeStr + "limit=100");
            if (response.IsSuccessStatusCode)
            {
                pets = await response.Content.ReadAsAsync<PetResponse>();
            }
            return pets;
        }

        public static async Task<HttpStatusCode> DeletePetAsync(string petId)
        {
            HttpResponseMessage response = await client.DeleteAsync(
                $"pets/{petId}");
                
            return response.StatusCode;
        }

        // static void Main()
        // {
        //     RunAsync().GetAwaiter().GetResult();
        // }

        public static async Task RunAsync()
        {
            // Update port # in the following line.
            // client.BaseAddress = new Uri("https://hieu.azure-api.net/petronas/pets/");
            // client.DefaultRequestHeaders.Accept.Clear();
            // client.DefaultRequestHeaders.Accept.Add(
            //     new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                // Create a new product
                Pet pet = new Pet
                {
                    animal_type = "cat",
                    created = DateTime.Now,
                    id = "123",
                    name = "Susie",
                    tags = new Tags(),
                };
                var petId = "3";

                // Get the product
                pet = await GetPetAsync(petId);
                ShowProduct(pet);

                var url = await CreatePetAsync(petId,pet);
                Console.WriteLine($"Created at {url}");


                // Get the product
                pet = await GetPetAsync(petId);
                ShowProduct(pet);

                // Update the product
                Console.WriteLine("Updating price...");
                pet.name = "cat1";
                await CreatePetAsync(petId, pet);

                // Get the updated product
                pet = await GetPetAsync(petId);
                ShowProduct(pet);

                // Delete the product
                var statusCode = await DeletePetAsync(petId);
                Console.WriteLine($"Deleted (HTTP Status = {(int)statusCode})");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
    
}