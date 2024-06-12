using System;

//Esta clase tiene el userID del usuario al que le pertenece el codigo
[Serializable]
public class CodigosUsuario
{
    public string UserID;

    public CodigosUsuario(string userID)
    {
        UserID = userID;
    }
}