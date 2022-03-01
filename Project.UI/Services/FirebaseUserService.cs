using Firebase.Auth;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.UI
{
	internal class FirebaseUserService
	{
		// Constants for firebase
		private static readonly string FirebaseAPIKey = "AIzaSyCoFlvXjEzrtKKcy9KC-49SFglJq2-9LiU";
		private static readonly string FirebaseDatabaseUrl = "https://maui-project-test-default-rtdb.europe-west1.firebasedatabase.app/";

		// Instances of the firebase client and auth
        public static FirebaseClient FirebaseClient { get; private set; }
		public static FirebaseAuthProvider AuthProvider { get; private set; }

		// Application properties to save data
		public static User SessionUser { get; set; }

		private const string TokenKey = "TokenKey";
		public static string SessionToken
		{
			get => Preferences.Get(TokenKey, string.Empty);
			set { Preferences.Set(TokenKey, value); }
		}

		/// <summary>
		/// Static Constructor
		/// </summary>
		static FirebaseUserService()
		{
			FirebaseClient = new FirebaseClient(FirebaseDatabaseUrl);

			AuthProvider = new FirebaseAuthProvider(
				new FirebaseConfig(FirebaseAPIKey));
		}

		/// <summary>
		/// Check if there is a valid token
		/// </summary>
		public static bool IsTokenValid()
		{
			if (string.IsNullOrEmpty(SessionToken))
				return false;

			return Task.Run(isValidateToken).Result;
		}

		/// <summary>
		/// check if the latest token is valid
		/// </summary>
		private static async Task<bool> isValidateToken()
		{
			try
			{
				var user = await AuthProvider.GetUserAsync(SessionToken);
				SessionUser = user;
				return true;
			}
			catch (Exception)
			{
				SessionToken = string.Empty;
				SessionUser = null;
			}

			return false;
		}
	}
}
