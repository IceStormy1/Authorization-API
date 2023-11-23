
using Client.Services;
using IdentityModel.Client;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Models;


namespace Client.Pages
{
    public partial class CoffeeShops
    {
        private List<CoffeeShopModel> Shops = new();
        [Inject] private HttpClient HttpClient { get; set; }
        [Inject] private IConfiguration Config { get; set; }
        [Inject] private ITokenService TokenService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var tokenResponse = await TokenService.GetToken("TheaterApi.read");
            HttpClient.SetBearerToken(tokenResponse.AccessToken);

            //var result = await HttpClient.GetAsync(Config["apiUrl"] + "/api/CoffeeShop");

            //if (result.IsSuccessStatusCode)
            //{
            //  Shops = await result.Content.ReadFromJsonAsync<List<CoffeeShopModel>>();
            //}

            Shops = new List<CoffeeShopModel>();
        }
    }
}
