using Microsoft.AspNetCore.Components;

namespace Authorization.UI.Shared
{
	public partial class RedirectToLogin
	{
		[Inject] public NavigationManager Navigation { get; set; }

		protected override async Task OnInitializedAsync()
		{
			Navigation.NavigateTo($"/login?redirectUri={Uri.EscapeDataString(Navigation.Uri)}", true);
		}
	}
}
