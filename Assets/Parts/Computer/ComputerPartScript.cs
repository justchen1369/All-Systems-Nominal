using UnityEngine;
using System.Collections;
using System.Linq;

public class ComputerPartScript : MonoBehaviour {
	public Peripheral[] connectedPeripherals;
	private string program = "";
	public string Program {
		get { return program;}
		set {
			program = value;
			program_pointer = 0;
			Output = "";
			memory = new System.Collections.Generic.Dictionary<string, int>();
			peripherals = new System.Collections.Generic.Dictionary<string, int[]> ();
			string s = "";
			bool isString = false;
			foreach (char c in value) {
				if (char.IsLetterOrDigit (c) || c == ' ' || c == '"' || c == ';') {
					if (c == '"') {
						isString = !isString;
					} else {
						if (isString) {
							s += char.ConvertToUtf32(char.ToString(c), 0) + " ";
						} else {
							s += char.ToString (c);
						}
					}
				}
			}
			s = s.Trim (new char[1] { '\n' });
			rawProgram = s.Split(new char[] { ';' });
			for (int i = 0; i < rawProgram.Length; i++) {
				rawProgram [i] = rawProgram [i].TrimStart (new char[1] { ' ' }).TrimEnd (new char[1] { ' ' });
			}
		}
	}
	public string Output;

	public string[] rawProgram;
	public System.Collections.Generic.Dictionary<string, int> memory = new System.Collections.Generic.Dictionary<string, int>();
	public System.Collections.Generic.Dictionary<string, int[]> peripherals = new System.Collections.Generic.Dictionary<string, int[]> ();
	public int clockSpeed;
	public int NumberSize;

	public float powerConsumption;
	public float ThermalEnergyGeneration;

	int getValue(string address) {
		if (memory.ContainsKey (address)) {
			return memory [address];
		} else {
			return 0;
		}
	}

	int getValue(int value) {
		return value;
	}

	void setValue(string address, int value) {
		if (memory.ContainsKey (address)) {
			memory [address] = value % NumberSize;
		} else {
			memory.Add (address, value % NumberSize);
		}
	}

	int getLitteral(string value) {
		int i = 0;
		if (int.TryParse(value, out i)) {
			return (i);
		} else {
			return (getValue (value));
		}
	}

	public int program_pointer = 0;
	private int delay = 0;

	// Update is called once per frame
	void FixedUpdate () {
		BasePartBehaviour bpb = gameObject.GetComponent<BasePartBehaviour> ();
		RecourseConnection rc = gameObject.GetComponent<RecourseConnection> ();
			
		foreach (string k in peripherals.Keys) {
			if (peripherals [k] [0] < connectedPeripherals.Length) {
				if (connectedPeripherals [peripherals [k] [0]] != null) {
					setValue (k, connectedPeripherals [peripherals [k] [0]].Ports [peripherals [k] [1]]);
				}
			}
		}
		int tick = 0;
		while (tick < clockSpeed && program_pointer < rawProgram.Length && delay < 1 && rc.RecourseAvailability(powerConsumption) >= powerConsumption) {
			bpb.Thermal.ThermalEnergy += ThermalEnergyGeneration;
			rc.TakeRecourse (powerConsumption);

			string[] instruction = rawProgram [program_pointer].Split (new char[1] { ' ' });

			switch (instruction [0].ToLower ()) {

			case "print":
				for (int i = 1; i < instruction.Length; i++) {
					Output += char.ConvertFromUtf32 (getLitteral (instruction [i]));
				}
				break;

			case "println":
				for (int i = 1; i < instruction.Length; i++) {
					Output += char.ConvertFromUtf32 (getLitteral (instruction [i]));
				}
				Output += "\n";
				break;

			case "printi":
				Output += getLitteral (instruction [1]).ToString ();
				break;

			case "set":
				setValue (instruction [1], getLitteral (instruction [2]));
				break;

			case "add":
				setValue (instruction [1], getValue (instruction [1]) + getLitteral (instruction [2]));
				break;
		
			case "sub":
				setValue (instruction [1], getValue (instruction [1]) - getLitteral (instruction [2]));
				break;

			case "jump":
				program_pointer = getLitteral (instruction [1]) - 1; 
				break;
		
			case "tag":
				setValue (instruction [1], program_pointer);
				break;

			case "if":
				switch (instruction [2]) {
				case "equals":
					if (getLitteral (instruction [1]) != getLitteral (instruction [3])) {
						program_pointer++;
					}
					break;
			
				case "notequals":
					if (getLitteral (instruction [1]) == getLitteral (instruction [3])) {
						program_pointer++;
					}
					break;
			
				case "greaterthan":
					if (getLitteral (instruction [1]) < getLitteral (instruction [3])) {
						program_pointer++;
					}
					break;

				case "lessthan":
					if (getLitteral (instruction [1]) > getLitteral (instruction [3])) {
						program_pointer++;
					}
					break;
				}
				break;

			case "periph":
				peripherals.Add (instruction [1], new int[] { getLitteral (instruction [2]), getLitteral (instruction [3]) });
				break;

			case "delay":
				delay = getLitteral (instruction [1]);
				break;

			case "clear":
				Output = "";
				break;
			}
			program_pointer++;
			tick++;
		}
		if (delay > 0) {
			delay -= 1;
		}
		foreach (string k in peripherals.Keys) {
			if (peripherals [k] [0] < connectedPeripherals.Length) {
				if (connectedPeripherals [peripherals [k] [0]] != null) {
					connectedPeripherals [peripherals [k] [0]].Ports [peripherals [k] [1]] = getValue (k);
				}
			}
		}
	}
}
