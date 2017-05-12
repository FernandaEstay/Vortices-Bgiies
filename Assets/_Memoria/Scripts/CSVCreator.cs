using System;
using System.IO;
using System.Text;
using Gamelogic;
using UnityEngine;

namespace Memoria
{
	public class CsvCreator
	{
		private readonly string _filePath;
		private readonly int _actualPersonId;

		private const string PersonId = "PersonId";

		public CsvCreator(string filePath)
		{
			_filePath = filePath;

			_actualPersonId = GLPlayerPrefs.GetInt("Config", "UserID");

            try
            {
                AddLines("UserID", "-");
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            
		}

		public bool AddLines(string action, string objectId)
		{
			var csv = new StringBuilder();

			var actualHour = DateTime.Now.TimeOfDay;
			var actualTimestamp = DateTime.Now.Date.ToShortDateString();
			var newLine = string.Format("{0},{1},{2},{3},{4}", _actualPersonId, actualHour, actualTimestamp, action, objectId);
			csv.AppendLine(newLine);

            try {
                File.AppendAllText(_filePath, csv.ToString());
            }
            catch(Exception e)
            {
                Debug.Log(e);
            }
			

			return true;
		}
	}
}