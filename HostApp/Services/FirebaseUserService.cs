using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostApp
{

	// TODO: Create an IUserService and encapsulate all the AuthProvider methods to what I will use
	internal class FirebaseUserService
	{
		// Constants for firebase
		private static readonly string FirebaseDatabaseUrl = "https://maui-project-test-default-rtdb.europe-west1.firebasedatabase.app/";

		// Instances of the firebase client and auth
        public static FirebaseClient FirebaseClient { get; private set; }

		/// <summary>
		/// Static Constructor
		/// </summary>
		static FirebaseUserService()
		{
			FirebaseClient = new FirebaseClient(FirebaseDatabaseUrl);
		}
	}
}
