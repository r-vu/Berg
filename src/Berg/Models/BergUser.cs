using Microsoft.AspNetCore.Identity;
using System.Collections;
using System.Collections.Generic;

namespace Berg.Models {
    // Add profile data for application users by adding properties to the BergUser class
    public class BergUser : IdentityUser {

        public BergUser() : base() { }

        public BergUser(string name) : base(name) { }

    }
}
