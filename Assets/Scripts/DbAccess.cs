using UnityEngine;
using System;
using System.Data;
using System.Collections;
using Mono.Data.Sqlite;

public class DbAccess : MonoBehaviour {

    string connection = "URI=file:data";
    IDbConnection dbcon;
    IDbCommand dbcmd;
    IDataReader reader;

    void OpenDB()
    {
        dbcon = new SqliteConnection(connection);
        dbcon.Open();
    }

    void CloseDB()
    {
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbcon.Close();
        dbcon = null;
    }

    IDataReader BasicQuery(string q) 
    {
        dbcmd = dbcon.CreateCommand();
        dbcmd.CommandText = q;
        reader = dbcmd.ExecuteReader();
        return reader;
    }

    void BasicUpdate(string q)
    {
        dbcmd = dbcon.CreateCommand();
        dbcmd.CommandText = q;
        dbcmd.ExecuteNonQuery();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
