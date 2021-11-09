using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.Navigations
{
    public partial class SfBreadcrumb : SfBaseComponent
    {

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            activeItem = ActiveItem;
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            BreadcrumbClass = "e-breadcrumb e-control e-lib";
            if (htmlAttributes != null)
            {
                if (htmlAttributes.ContainsKey("class"))
                {
                    BreadcrumbClass = SfBaseUtils.AddClass(BreadcrumbClass, (htmlAttributes["class"] as string));
                    htmlAttributes.Remove("class");
                }
                if(htmlAttributes.ContainsKey("id"))
                {
                    IdValue = htmlAttributes["id"] as string;
                }
            }
            if (!string.IsNullOrEmpty(Width))
            {
                styleAttributes = "width: " + Width;
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (Items == null)
                {
                    string prevUri;
                    string[] uri;
                    if (Url != null)
                    {
                        Uri givenUri = new Uri(Url);
                        prevUri = givenUri.GetLeftPart(System.UriPartial.Authority) + "/";
                        uri = givenUri.AbsoluteUri.Split(prevUri)[1].Split("/");
                    }
                    else
                    {
                        prevUri = navigationManager.BaseUri;
                        uri = navigationManager.Uri.Split(prevUri)[1].Split("/");
                    }
                    List<BreadcrumbItem> items = new List<BreadcrumbItem>();
                    BreadcrumbItem item = new BreadcrumbItem();
                    item.UpdateChildProperties("iconCss", "e-icons e-home");
                    item.UpdateChildProperties("url", prevUri);
                    items.Add(item);
                    for (int i = 0; i < uri.Length; i++)
                    {
                        if (uri[i].Length > 0)
                        {
                            item = new BreadcrumbItem();
                            item.UpdateChildProperties("text", uri[i]);
                            item.UpdateChildProperties("url", prevUri + uri[i]);
                            items.Add(item);
                            prevUri += uri[i] + "/";
                        }
                    }
                    Items = items;
                    StateHasChanged();
                }
                if (EnablePersistence)
                {
                    var localStorageValue = await InvokeMethod<string>("window.localStorage.getItem", false, new object[] { IdValue });
                    localStorageValue = string.IsNullOrEmpty(localStorageValue) ? null : localStorageValue;
                    if (localStorageValue != null && localStorageValue != "null")
                    {
                        var persistValue = (string)SfBaseUtils.ChangeType(localStorageValue, typeof(string));
                        if (persistValue != null)
                        {
                            ActiveItem = persistValue;
                            StateHasChanged();
                        }
                    }
                }
            }
            else if (PropertyChanges.Count > 0)
            {
                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }
    }
}