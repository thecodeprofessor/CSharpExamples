using Firebase.Database;
using Firebase.Database.Query;

namespace ExampleFirebaseRealtimeDatabaseConsole
{
    //NuGet Packages: FirebaseDatabase.net by Step Up Labs, Inc.

    //Console: https://console.firebase.google.com/u/0/project/_/firestore/data
    //API Keys: https://firebase.google.com/docs/projects/api-keys
    //Credentials: https://console.cloud.google.com/apis/credentials?pli=1
    //Rules

    // Pet class definition
    public class Pet
    {
        public string Name { get; set; }
        public string Species { get; set; }
        public int Age { get; set; }
        public double Price { get; set; }
        public string Color { get; set; }
    }

    class Program
    {
        public static string key = "keyhere";
        public static string url = "urlhere";

        static async Task Main(string[] args)
        {
            // Initialize Firebase
            var client = new FirebaseClient(
                url,
                new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(key)
                });

            // Create a new pet
            var pet = new Pet
            {
                Name = "Buddy",
                Species = "Dog",
                Age = 3,
                Price = 250.0,
                Color = "Golden"
            };

            // Add the pet to the database
            var newPet = await client
                .Child("pets")
                .PostAsync(pet);
            Console.WriteLine($"1. Created pet with key: {newPet.Key}");

            // Retrieve the pet from the database
            var existingPet = await client
                .Child("pets")
                .Child(newPet.Key)
                .OnceSingleAsync<Pet>();
            Console.WriteLine($"2. Retrieved pet: {existingPet.Name}");

            // Update the pet's age
            existingPet.Age = 4;
            await client
                .Child("pets")
                .Child(newPet.Key)
                .PutAsync(existingPet);
            Console.WriteLine($"3. Updated pet's age to: {existingPet.Age}");

            // Delete the pet from the database
            await client
                .Child("pets")
                .Child(newPet.Key)
                .DeleteAsync();
            Console.WriteLine("4. Deleted pet from the database");
        }
    }
}