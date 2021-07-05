using System;
using System.Collections.Generic;
using System.Text;
using UnivIntel.GBN.Core.Models;

namespace UnivIntel.GBN.Core.Handlers
{
    static public class TokenStorage
    {
        private static IDictionary<Guid, UserSession> m_Tokens;

        public static void InitStorage()
        {
            //TODO: get tokens from database
            m_Tokens = new Dictionary<Guid, UserSession>();
        }

        public static bool AddToken(Guid id, UserSession session)
        {
            //TODO: insert token to database

            return m_Tokens.TryAdd(id, session);
        }

        public static UserSession GetUserSession(Guid id)
        {
            m_Tokens.TryGetValue(id, out var session);
            return session;
        }


    }
}
