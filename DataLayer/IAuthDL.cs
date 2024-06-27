
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shareplus.Model;


namespace Shareplus.DataLayer
{
    public interface IAuthDL
    {
        public Task<SignUpResponse> SignUp(SignUpRequest request);

        public Task<SignInResponse> SignIn(SignInRequest request);
    }
}
