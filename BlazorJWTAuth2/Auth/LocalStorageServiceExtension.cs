using Blazored.LocalStorage;
using System.Text.Json;
using System.Text;

namespace BlazorJWTAuth2.Auth
{
    public static class LocalStorageServiceExtension
    {
        public static AesEncryption aesEncryption = new();
        public static async Task SaveItemEncryptedAsync<T>(this ILocalStorageService localStorageService, string key, T item)
        {
            var itemJson = JsonSerializer.Serialize(item);
            var itemJsonBytes = Encoding.UTF8.GetBytes(itemJson);
            itemJsonBytes = aesEncryption.Encrypt(itemJsonBytes);

            var base64Json = Convert.ToBase64String(itemJsonBytes);
            await localStorageService.SetItemAsync(key, base64Json);
        }

        public static async Task<T?> ReadEncryptedItemAsync<T>(this ILocalStorageService localStorageService, string key)
        {
            try
            {
                var base64Json = await localStorageService.GetItemAsync<string>(key);
                var itemJsonBytes = Convert.FromBase64String(base64Json);

                itemJsonBytes = aesEncryption.Decrypt(itemJsonBytes);
                var itemJson = Encoding.UTF8.GetString(itemJsonBytes);
                var item = JsonSerializer.Deserialize<T>(itemJson);
                return item;
            }
            catch
            {
                return default;
            }
        }
    }
}
