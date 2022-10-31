using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using PetApi;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit;

namespace PetApiTest.ControllerTest;

public class PetController
{
    [Fact]
    public async void Should_add_new_pet_to_system_seccessfully()
    {
        //given
        var application = new WebApplicationFactory<Program>();
        var httpClient = application.CreateClient();
        /*
          Method: POST
          URI: /api/addNewPet
          Body:
          {
            "name":"kitty",
            "type":"cat",
            "color":"white",
            "price":1000
          }
         */
        var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: 1000);
        var serializeObject = JsonConvert.SerializeObject(pet); //序列化
        var postBody = new StringContent(serializeObject, Encoding.UTF8, mediaType: "application/json");
        var response = await httpClient.PostAsync("/api/addNewPet", postBody);
        //then
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        var savedPet = JsonConvert.DeserializeObject<Pet>(responseBody);
        Assert.Equal(pet, savedPet);
    }

    [Fact]
    public async void Should_get_all_pets_from_system_seccessfully()
    {
        //given
        var application = new WebApplicationFactory<Program>();
        var httpClient = application.CreateClient();
        await httpClient.DeleteAsync("/api/deleteAllPets");
        /*
          Method: Get
          URI: /api/addNewPet
          Body:
          {
            "name":"kitty",
            "type":"cat",
            "color":"white",
            "price":1000
          }
         */
        var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: 1000);
        var serializeObject = JsonConvert.SerializeObject(pet); //序列化
        var postBody = new StringContent(serializeObject, Encoding.UTF8, mediaType: "application/json");
        await httpClient.PostAsync("/api/addNewPet", postBody);
        //when
        var response = await httpClient.GetAsync("/api/getAllPets");
        //then
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        var allPets = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
        Assert.Equal(pet, allPets[0]);
    }

    [Fact]
    public async void Should_get_pet_by_name_from_system_seccessfully()
    {
        //given
        var application = new WebApplicationFactory<Program>();
        var httpClient = application.CreateClient();
        await httpClient.DeleteAsync("/api/deleteAllPets");
        /*
          Method: Get
          URI: /api/addNewPet
          Body:
          {
            "name":"kitty",
            "type":"cat",
            "color":"white",
            "price":1000
          }
         */
        var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: 1000);
        var serializeObject = JsonConvert.SerializeObject(pet); //序列化
        var postBody = new StringContent(serializeObject, Encoding.UTF8, mediaType: "application/json");
        await httpClient.PostAsync("/api/addNewPet", postBody);
        //when
        var response = await httpClient.GetAsync("/api/findPetByName?name=Kitty");
        //then
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        var getPet = JsonConvert.DeserializeObject<Pet>(responseBody);
        Assert.Equal(pet, getPet);
    }
}