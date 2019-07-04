/*
 * NP Web Assignment
 * Authenticated Session Token
*/

using Jose;
using System;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Authentication;

using folio.Models;
using folio.FormModels;

namespace folio.Services.Auth
{
    // defines an authenticated session
    public class Session
    {  
        /* private constants */
        private const string JWTIssuer = "folio.api";
        private const string JWTAudience = "folio.api";

        /* properties */
        public string EmailAddr { get; set; }
        public string Hash { get; set; }
        public Dictionary<string, string> MetaData { get; set; }
        private long IssueTimeStamp { get; set; }
        private long Expiry { get; set; }
    
        /* constructor */
        // create an authenticated session for the user given by their EmailAddr
        public Session(string EmailAddr)
        {
            this.EmailAddr = EmailAddr;
            this.MetaData = new Dictionary<string, string>();

            // setup issue timestamp and expiry time of the session
            DateTime IssueTimeStamp = DateTime.Now;
            this.IssueTimeStamp = Session.ConvertToUnixTimestamp(IssueTimeStamp);
            DateTime expiry = IssueTimeStamp.AddDays(7); // session expires in 7 days
            this.Expiry = Session.ConvertToUnixTimestamp(expiry);
        }
        
        /* JSON web token conversion */
        // reconstruct a session from JSON web token 
        protected Session() {}
        public static Session FromJWT(string token, byte[] secretKey)
        {
            // unpack payload from token
            string payloadJson = Jose.JWT.Decode(token, secretKey);
            dynamic payload = JObject.Parse(payloadJson);
        
            string emailAddr = payload.sub;
            string issuer = payload.iss;
            string audience = payload.aud;
            long issueTimeStamp = payload.iat;
            long expiry = payload.exp;
            Dictionary<string, string> meta = payload.meta
                .ToObject<Dictionary<string, string>>();

            // check if the token is still valid 
            // validate that the token has not yet expired
            DateTime expiryDateTime = Session.ConvertFromUnixTimestamp(expiry);
            if(DateTime.Now >= expiryDateTime)
                throw new AuthenticationException("JWT token has already expired");
            // validate audience and issuer of the token
            if(audience != Session.JWTAudience || issuer != Session.JWTIssuer)
                throw new AuthenticationException(
                        "JWT token has invalid audience and/or issuer");
        
            // recreate session based on payload 
            return new Session
            {
                EmailAddr = emailAddr,
                MetaData = meta,
                Expiry = expiry,
                IssueTimeStamp = issueTimeStamp
            };
        }
    
    
        // Convert & return session as a JSON web token.
        // Using the given secret key to sign and encrypt the token
        public string ToJWT(byte[] secretKey)
        {
            // Construct dictionary of contents of session
            Dictionary<string, object> payload  = new Dictionary<string, object>
            {
                { "aud", Session.JWTAudience }, // audience
                { "iss", Session.JWTIssuer }, // issuer
                { "sub", this.EmailAddr }, // subject 
                { "exp", this.Expiry }, // expiry
                { "iat", this.IssueTimeStamp }, // issued at timestamp
                { "meta", this.MetaData }
            };
    
            // Encode the contents as a JWT
            string token = JWT.Encode(payload, secretKey, JweAlgorithm.A256KW, 
                    JweEncryption.A256CBC_HS512);
            return token;
        }
        
        /* object equality */
        public override bool Equals(object obj)
        {
            // check for null and type missmatch
            if(obj == null) return false;
            if (this.GetType() != obj.GetType()) return false;
            
            // check object properties
            Session other = (Session)obj;
            if(other.EmailAddr != other.EmailAddr) return false;
            else if(other.MetaData != other.MetaData) return false;
            else if(other.IssueTimeStamp != other.IssueTimeStamp) return false;
            else if(other.Expiry != other.Expiry) return false;

            return true;
        }
    
        /* private utilities */
        // utilities to convert to & from seconds since epoch and datetime
        private static DateTime ConvertFromUnixTimestamp(long timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return origin.AddSeconds(timestamp);
        }

        private static long ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return (long)Math.Floor(diff.TotalSeconds);
        }
    
        // hash the given string input using the SHA256 hashing algorithm
        private static string ComputeHash(string input)
        {
            // extract bytes from the given string input 
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            // perform hashing using SHA256 
            SHA256Managed hashAlgorithm = new SHA256Managed();
            byte[] hashBytes = hashAlgorithm.ComputeHash(inputBytes);

            // convert hash to hexdecimal string
            string hash = string.Join("", hashBytes
                    .Select(b => string.Format("{0:x2}", b)));
            return hash;
        }
    }
}
