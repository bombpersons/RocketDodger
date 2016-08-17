using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class WaveManager : MonoBehaviour
{

    // Public class
    public class Wave
    {
        // Public vars
        public int EntryCount
        {
            get { return entries.Count; }
        }
        public List<WaveEntry> Entries
        {
            get
            {
                return entries;
            }
        }

        // Private vars
        List<WaveEntry> entries = new List<WaveEntry>();

        // Constructor
        public Wave()
        {
        }

        // Sort
        public void Sort()
        {
            entries.Sort();
        }

        // Get an entry
        public WaveEntry GetEntry(int _i)
        {
            if (_i < EntryCount || _i >= 0)
            {
                return entries[_i];
            }
            return null;
        }

        // Save a wave to a file.
        public void Save(string _path)
        {
            // Try and open that file.
            FileStream stream = new FileStream(_path, FileMode.CreateNew, FileAccess.Write);
            if (stream == null)
                return;

            // Create the binarywriter.
            BinaryWriter writer = new BinaryWriter(stream);
            Save(writer);

            // Close everything
            writer.Close();
            stream.Close();
        }
        public void Save(BinaryWriter _writer)
        {
            // Write how many entries there are
            _writer.Write(entries.Count);

            // Loop through all of the entries and write them.
            foreach (WaveEntry entry in entries)
            {
                entry.Save(_writer);
            }
        }

        // Load a wave from a file.
        public void Load(string _path)
        {
            // Try and open that file.
            FileStream stream = new FileStream(_path, FileMode.Open, FileAccess.Read);
            if (stream == null)
                return;

            // Create the binarywriter.
            BinaryReader reader = new BinaryReader(stream);
            Load(reader);

            // Close everything
            reader.Close();
            stream.Close();
        }
        public void Load(BinaryReader _reader)
        {
            // Clear the entries
            entries.Clear();

            // Read how many entries there are
            int count = _reader.ReadInt32();

            // Loop through all of the entries and read them.
            for (int i = 0; i < count; ++i)
            {
                WaveEntry entry = new WaveEntry();
                entry.Load(_reader);
                entries.Add(entry);
            }
        }
    }

    public class WaveEntry : System.IComparable<WaveEntry>
    {
        // Public vars
        public Vector3 Position;
        //public float Angle;
        public string Type;
        public float Time;

        // Private vars

        // Constructor
        public WaveEntry()
        {

        }

        // Compare to (so we can sort by time easily).
        public int CompareTo(WaveEntry _entry)
        {
            return Time.CompareTo(_entry.Time);
        }

        // Save to a binary writer.
        public void Save(BinaryWriter _writer)
        {
            // Write the position.
            _writer.Write(Position.x);
            _writer.Write(Position.y);
            _writer.Write(Position.z);

            // Write the Type
            _writer.Write(Type);

            // Write the time this rocket should spawn.
            _writer.Write(this.Time);
        }

        // Load from a binary reader
        public void Load(BinaryReader _reader)
        {
            // Read the position
            float x = _reader.ReadSingle();
            float y = _reader.ReadSingle();
            float z = _reader.ReadSingle();
            Position = new Vector3(x, y, z);

            // Read the type
            Type = _reader.ReadString();

            // Read the time
            this.Time = _reader.ReadSingle();
        }
    }

    // ListEntry for rocket difficulty numbers
    [System.Serializable]
    public class RocketDiffListEntry
    {
        public string RocketType;
        public float Difficulty;
    }

    // Public vars
    public bool GenerateRandomWaves = false;
    public GameObject TextPrefab;
    public List<RocketDiffListEntry> RocketDifficultyList = new List<RocketDiffListEntry>();
    public static Dictionary<string, float> RocketDifficulty = new Dictionary<string, float>();
    public static int WaveNum
    {
        get
        {
            return waveNum;
        }
    }

    // Private vars
    static Wave currentWave;
    static int currentRocket;
    static float currentTimer;
    static GameObject textPrefab;
    static int waveNum;



    // Random generation values
    static float difficultyTimeModifier = 10; // How much to drop the percieved difficulty of a rocket per second after the last rocket was spawned.


    // Stop
    public static void Stop()
    {
        currentWave = null;
        currentRocket = 0;
        currentTimer = 0;

        // set to the wave to the nearst checkpoint of 5
        var fw = PlayerPrefs.GetInt("FurthestWave");
        waveNum = fw - (fw % 5);

        if (waveNum == 0)
            waveNum = 1;
    }

    // Set the wave to spawn.
    public static void SpawnWave(Wave _wave)
    {
        // Set the current wave and reset the timer.
        currentWave = _wave;
        currentRocket = 0;
        currentTimer = 0;

        // Text.
        GameObject text = (GameObject)Instantiate(textPrefab);
        text.GetComponent<GUIText>().text = "Wave " + waveNum.ToString();
        

        // Set the position.
        //		text.GetComponent<BounceSizer>().Bounce(20.0f);
        //		text.GetComponent<DeathTimer>().Timer = 10.0f;
        //text.transform.position = new Vector3(0.5f, 0.5f, 0);
    }

    // Generate a wave worth a certain point.
    public static Wave GenerateRandomWave(float _num)
    {

        System.Random detRand = new System.Random(1337 + waveNum);//determanistic random number
                                                                  // Using the difficulty number, calculate a few values

        // How closely the rockets will be spawned together.
        float spawnDensity = (1 / (_num + 1)) * 50;
        if (spawnDensity < 0.01f) spawnDensity = 0.01f;
        float lowRange = spawnDensity - (spawnDensity / 4);
        float highRange = spawnDensity + (spawnDensity / 4);

        // The max difficulty of a single rocket.
        float maxDiff = _num / 2;

        // Start calculating the wave.
        Wave wave = new Wave();
        float time = 5.0f;
        bool firstRocket = true;
        while (_num > 0)
        {
            WaveEntry entry = new WaveEntry();
            float angle = RandInRange(detRand, -Mathf.PI, Mathf.PI);
            entry.Position = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));

            // Get a random rocket type.
            int type = detRand.Next(0, 4);
            switch (type)
            {
                case 0:
                    entry.Type = "PointAtRocket";
                    break;
                case 1:
                    entry.Type = "ProportionalRocket";
                    break;
                case 2:
                    entry.Type = "Leading";
                    break;
                case 3:
                    entry.Type = "ClusterRocket";
                    break;
                default:
                    entry.Type = "PointAtRocket";
                    break;
            }

            // Get a random time.
            float extraTime = RandInRange(detRand, lowRange, highRange);

            // Calculate the difficulty of this rocket.
            float diff = GetDifficulty(entry, extraTime, lowRange, highRange);

            // Check if we can spawn this rocket.
            while (diff > maxDiff)
            {
                // Try increasing the time to spawn it.
                extraTime += (highRange - lowRange) / 10;

                // If we are at the max extratime
                if (extraTime > highRange)
                {
                    extraTime = highRange;

                    // Try making the rocket easier.
                    bool found = false;
                    foreach (KeyValuePair<string, float> pair in RocketDifficulty)
                    {
                        if (pair.Value < RocketDifficulty[entry.Type])
                        {
                            entry.Type = pair.Key;
                            found = true;
                            break;
                        }
                    }
                    if (found == false)
                    {
                        // Nothing else we can do to make it any easier, just set _num to 0 and start the next wave.
                        _num = 0;
                        return wave;
                    }
                }

                // Recalculate the difficulty
                diff = GetDifficulty(entry, extraTime, lowRange, highRange);
            }

            // Take the diff away.
            _num -= diff;

            // Add to the wave
            if (firstRocket)
            { // If this is the first rocket, then spawn it first regardless of the time that was previously calculated.
                firstRocket = false;
            }
            else
            {
                time += extraTime;
            }
            entry.Time = time;
            wave.Entries.Add(entry);
        }
        return wave;
    }

    private static float RandInRange(System.Random _detRand, float _min, float _max)
    {
        // Get a random angle.
        return _min + (_max - _min) * (float)_detRand.NextDouble();
    }

    // Calculate a rockets difficulty
    public static float GetDifficulty(WaveEntry _entry, float _timeSinceLastSpawn, float _lowRange, float _highRange)
    {
        float diff = RocketDifficulty[_entry.Type]; // The base difficulty.
        diff += (1 - ((_timeSinceLastSpawn - _lowRange) / (_highRange - _lowRange))) * diff; // The difficulty increases the shorter the time between them.
        return diff;
    }

    // Get a rocket type from a number.


    // Start
    public void Start()
    {

        // Create the rocket difficulty table.
        if (RocketDifficulty.Count == 0)
        {
            foreach (RocketDiffListEntry entry in RocketDifficultyList)
            {
                RocketDifficulty.Add(entry.RocketType, entry.Difficulty);
            }
        }

        if(!PlayerPrefs.HasKey("FurthestWave"))
            PlayerPrefs.SetInt("FurthestWave", 1);

        // Set the static reference of the prefab
        textPrefab = TextPrefab;

        //  Reset things.
        Stop();
    }

    // Update
    public void Update()
    {
        // If the player is dead then don't do anything.
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null)
            return;
        if (player.GetComponent<PlayerDie>().IsDead)
            return;

#if UNITY_EDITOR
        // Press R to reset saved FurthestWave to 0
        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerPrefs.DeleteAll();
            waveNum = 0;

        }
#endif

        // Generate random waves?
        if (GenerateRandomWaves)
        {
            // If there isn't a wave, generate one.
            if (currentWave == null)
            {
                if (GameObject.FindGameObjectsWithTag("Rocket").Length == 0)
                {
                    float cNum = waveNum / 5; //checkpoint Numumber
                    float lSCp = waveNum % 5; // leveles Sinse Checkpoint

                    // The dificulty ramps up quickly to begin with but later checkpoints have smaller dificulty steps.
                    // http://www.wolframalpha.com/input/?i=plot+%5BLog((x%2B+1)+%2F+10.0+%2B+1)+*+100.0%5D+for+x%3D0..100
                    float difucyltyRamping = Mathf.Log((cNum + 1) / 10.0f + 1) * 100.0f;

                    // Each consecutive level since the last checkpoint goes up by 10 dificulty points
                    //http://www.wolframalpha.com/input/?i=plot+%5B(Log(((x%2F+5)+%2B+1)+%2F+10.0+%2B+1)+*+100.0+%2B+(MOD(x,5))+*+10.0)%5D+for+x%3D0..500
                    float difficulty = difucyltyRamping + lSCp * 10.0f;

                    SpawnWave(GenerateRandomWave(difficulty));
                }
            }
        }

        if (currentWave != null)
        {
            if (currentRocket < currentWave.EntryCount)
            {
                currentTimer += Time.deltaTime;
                if (currentWave.Entries[currentRocket].Time < currentTimer)
                {
                    // Spawn this rocket.
                    SpawnRocket(currentWave.GetEntry(currentRocket));

                    // Inc the current rocket.
                    currentRocket++;
                }
            }
            else if (GameObject.FindGameObjectsWithTag("Rocket").Length == 0)
            {
                currentWave = null;
                waveNum++;
            }
        }
    }

    // Spawn a rocket.
    public void SpawnRocket(WaveEntry _entry)
    {
        // Get the correct prefab.
        GameObject prefab = (GameObject)Resources.Load("Prefabs/Rockets/" + _entry.Type, typeof(GameObject));

        Vector3 position = Camera.main.transform.position + (_entry.Position * Camera.main.orthographicSize * 2);
        position.y = 0;
        GameObject rocket = (GameObject)Instantiate(prefab, position, Quaternion.LookRotation(-_entry.Position));
        rocket.GetComponent<Rigidbody>().velocity = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().velocity;

    }

    // Called when the level is restarted.
    public void RestartLevel()
    {
        Stop();
    }

    // Save the score
    public static void SaveScore()
    {
        // Check what the previous high score was.
        int furthestWave = GetFurthestWave();

        // Set the previous wave's scores
        PlayerPrefs.SetInt("PreviousFurthestWave", furthestWave);

        // Set the current score if we have got something better.
        if (waveNum > furthestWave)
        {
            PlayerPrefs.SetInt("PreviousFurthestWave", furthestWave);
            PlayerPrefs.SetInt("FurthestWave", WaveManager.WaveNum);
        }
        PlayerPrefs.Save();
    }

    public static int GetFurthestWave()
    {
        return PlayerPrefs.GetInt("FurthestWave");
    }

    public static int GetPreviousFurthestWave()
    {
        return PlayerPrefs.GetInt("PreviousFurthestWave");
    }
}