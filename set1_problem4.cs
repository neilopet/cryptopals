using System;
using System.Collections.Generic;

public class Program
{
	public static byte[] FREQUENCY_TABLE = 
	{ 
		(byte)32,
		(byte)101, 
		(byte)116, 
		(byte)97, 
		(byte)111, 
		(byte)105 
	};
	
	public static void Main()
	{
		byte[] input = decodeHex("/* hex string */");
		
		bruteByFrequency( input, characterFrequencyCalc( input ) );
		
		/*
		for( int i = 0; i < 255; i++ )
		{
			
			Console.Write(String.Format("{0} :", i));
			printByteArrayAscii( decode( input, new byte[]{(byte)i} ) );
			Console.WriteLine("");
		}
		*/
	}
	
	public static void printByteArrayAscii( byte[] input )
	{
		foreach( byte b in input )
		{
			Console.Write( (char)b );
		}
	}
	
	public static byte[] decode( byte[] input, byte[] key )
	{
		byte[] result = new byte[ input.Length ];
		int offset = 0;
		
		for( int iterator = 0; iterator < (input.Length/2); iterator++ )
		{
			offset = iterator;
			if (iterator >= key.Length)
			{
				offset = (iterator % key.Length);
			}
			result[ iterator ] = (byte)(input[ iterator ] ^ key[ offset ]);
		}
		return result;
	}
	
	public static byte[] decodeHex( string s )
	{
		byte[] ret = new byte[ s.Length ];
		for(int i = 0, c = 0; i < s.Length; i+=2, c++) 
		{
			ret[c] = Convert.ToByte((s[i] + "" + s[i+1]), 16);
		}
		return ret;
	}
	
	public static Dictionary<byte, int> characterFrequencyCalc( byte[] input ) 
	{
		Dictionary<byte, int> count = new Dictionary<byte, int>();
		foreach(byte b in input)
		{
			if (count.ContainsKey(b))
			{
				count[b]++;
			}
			else
			{
				count.Add(b, 1);
			}
		}
		
		return count;
	}
	
	public static void bruteByFrequency( byte[] input, Dictionary<byte, int> frequencies )
	{				
		if (frequencies.Count < 1)
		{
			return;
		}
		
		int max_frequency = 0;
		byte frequent_byte = (byte)32;
		byte xorKey = 0x00;
		
		foreach( KeyValuePair<byte, int> kvp in frequencies )
		{
			if (kvp.Value > max_frequency)
			{
				max_frequency = kvp.Value;
				frequent_byte = kvp.Key;
			}
		}
		
		foreach( byte c in FREQUENCY_TABLE )
		{
			xorKey = (byte)(frequent_byte ^ c);
			Console.Write(String.Format("{0}: ", xorKey));
			printByteArrayAscii( decode( input, new byte[]{xorKey} ) );
			Console.WriteLine("\n");
		}
				
		frequencies.Remove(frequent_byte);
		
		bruteByFrequency( input, frequencies );
	}
}
