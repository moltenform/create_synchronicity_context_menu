//This file is part of Create Synchronicity.
//
//Create Synchronicity is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
//Create Synchronicity is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
//You should have received a copy of the GNU General Public License along with Create Synchronicity.  If not, see <http://www.gnu.org/licenses/>.
//Created by:	Clément Pit--Claudel.
//Web site:		http://synchronicity.sourceforge.net.

using System;
using IO = System.IO;
using Text = System.Text;
using System.Collections.Generic;

namespace Tests {
    class Program {
        //static string Input(string message) {
        //    Console.Write(message + " ");
        //    return Console.ReadLine();
        //}

        static void Usage() {
            Console.WriteLine("Usage: test-cs (option) (parameters)");
            Console.WriteLine("Options: compare   (source) (dest)");
            Console.WriteLine("         randomize (path)");
            throw new ArgumentException("Invalid command-line arguments.");
        }

        static void Main(string[] args) {
            if (args.Length == 0)
                Usage();

            switch (args[0]) {
                case "compare":
                    if (args.Length < 3)
                        Usage();

                    FS f1 = new FS(args[1]), f2 = new FS(args[2]);
                    f1.CompareTo(f2);
                    break;

                case "randomize":
                    if (args.Length < 2)
                        Usage();

                    string basepath = args[1];
                    if (!IO.Directory.Exists(basepath))
                        IO.Directory.CreateDirectory(basepath);

                    RandomFS fs = new RandomFS(basepath, 0, 200, 15, 3, 20, 3);
                    fs.build();
                    break;

                default:
                    Usage();
                    break;
            }
            Console.ReadLine();
        }
    }

    class FS {
        public struct Entry : IComparable<Entry> {
            public readonly string base_path;
            public readonly string relative_path;
            public readonly IO.FileAttributes attr;
            //public readonly DateTime date_created;
            public readonly DateTime date_last_modified;

            public Entry(string absolute_path, string _basepath) {
                attr = IO.File.GetAttributes(absolute_path);

                //date_created = IO.File.GetCreationTimeUtc(absolute_path);
                date_last_modified = IO.File.GetLastWriteTimeUtc(absolute_path);

                base_path = _basepath.Trim(IO.Path.DirectorySeparatorChar);
                relative_path = absolute_path.Substring(_basepath.Length).Trim(IO.Path.DirectorySeparatorChar);
            }

            public int CompareTo(Entry other) {
                // int bases = base_path.CompareTo(other.base_path); // Don't compare bases
                int relatives = relative_path.CompareTo(other.relative_path);
                int attributes = attr.CompareTo(other.attr);
                //  int dates_created = date_created.CompareTo(other.date_created);
                int dates_last_modified = date_last_modified.CompareTo(other.date_last_modified);

                return (relatives != 0 ? relatives : (attributes != 0 ? attributes : dates_last_modified)); //(bases != 0 ? bases : ... )
            }

            public static void CompareAttributes(FS.Entry e1, FS.Entry e2) {
                int attr1 = (int)e1.attr, attr2 = (int)e2.attr;

                int added = attr2 & (~attr1);
                int unchanged = attr1 & attr2;
                int removed = attr1 & (~attr2);

                //Console.WriteLine(" -> " + Convert.ToString(attr1, 2).PadLeft(16, '0'));
                //Console.WriteLine(" -> " + Convert.ToString(attr2, 2).PadLeft(16, '0'));

                if (attr1 != attr2) {
                    Console.WriteLine(" Attributes:");

                    if (removed != 0)
                        Console.WriteLine("  - " + ((IO.FileAttributes)removed).ToString());

                    if (unchanged != 0)
                        Console.WriteLine("  = " + ((IO.FileAttributes)unchanged).ToString());

                    if (added != 0)
                        Console.WriteLine("  + " + ((IO.FileAttributes)added).ToString());
                }

                Action<string, DateTime, DateTime> CompareDates = (label, date1, date2) => {
                    if (date1 != date2) {
                        Console.WriteLine(" " + label);
                        Console.WriteLine("  - " + date1.ToString("u"));
                        Console.WriteLine("  + " + date2.ToString("u"));
                    }
                };

                //CompareDates("Creation date:", e1.date_created, e2.date_created);
                CompareDates("Modification date:", e1.date_last_modified, e2.date_last_modified);
            }
        }

        protected readonly string basepath;
        protected List<Entry> entries;

        public FS(string base_path) {
            basepath = base_path;
            entries = new List<Entry>();
            scan(basepath);
        }

        private void scan(string path) {
            foreach (string file in IO.Directory.GetFiles(path))
                entries.Add(new Entry(file, basepath));

            foreach (string directory in IO.Directory.GetDirectories(path)) {
                entries.Add(new Entry(directory, basepath));
                scan(directory);
            }
        }

        public void CompareTo(FS other) {
            entries.Sort();
            other.entries.Sort();

            List<Entry> Added = new List<Entry>();
            List<Entry> Removed = new List<Entry>();
            List<Tuple<Entry, Entry>> Changed = new List<Tuple<Entry, Entry>>();

            int pos = 0, other_pos = 0;
            while (pos < entries.Count && other_pos < other.entries.Count) {
                FS.Entry elem = entries[pos];
                FS.Entry other_elem = other.entries[other_pos];

                int order = elem.relative_path.CompareTo(other_elem.relative_path);

                if (order == 0) {
                    pos++;
                    other_pos++;
                    if (elem.CompareTo(other_elem) != 0) Changed.Add(new Tuple<Entry, Entry>(elem, other_elem));
                }
                else if (order < 0) {
                    pos++;
                    Removed.Add(elem);
                }
                else {
                    other_pos++;
                    Added.Add(other_elem);
                }
            }

            while (pos < entries.Count)
                Removed.Add(entries[pos++]);

            while (other_pos < other.entries.Count)
                Added.Add(other.entries[other_pos++]);

            Added.ForEach(e => Console.WriteLine("+ " + e.relative_path));
            Removed.ForEach(e => Console.WriteLine("- " + e.relative_path));
            Changed.ForEach(t => { Console.WriteLine("~ " + t.Item1.relative_path); Entry.CompareAttributes(t.Item1, t.Item2); });
        }
    }

    class RandomFS : FS {
        private int _seed;
        private int _max_allowed_files;
        private int _max_allowed_folders;
        private int _max_depth;
        private int _avg_file_size;
        private int _file_size_delta;

        private readonly RandomGenerator randomizer;

        private string create_random_file(string parent_directory) {
            string name = randomizer.NextFileName();
            string path = IO.Path.Combine(parent_directory, name);

            int size = _avg_file_size + randomizer.Next(-_file_size_delta, _file_size_delta + 1);

            using (IO.Stream file = IO.File.Create(path)) {
                byte[] buffer = new byte[1024];
                for (int pos = 0; pos < size; pos++) {
                    randomizer.NextBytes(buffer);
                    file.Write(buffer, 0, 1024);
                }
            }

            IO.FileAttributes attr = randomizer.NextAttributes(false);
            System.IO.File.SetAttributes(path, attr);

            Console.WriteLine("Created file " + path + " with attributes " + attr.ToString());
            entries.Add(new Entry(path, basepath));
            return path;
        }

        private string create_random_folder(string parent_directory) {
            string name = randomizer.NextDirectoryName();
            string path = IO.Path.Combine(parent_directory, name);

            IO.Directory.CreateDirectory(path);

            IO.FileAttributes attr = randomizer.NextAttributes(true);
            IO.File.SetAttributes(path, attr);

            Console.WriteLine("Created folder " + path + " with attributes " + attr);
            entries.Add(new Entry(path, basepath));
            return path;
        }

        public RandomFS(string basepath, int seed, int max_allowed_files, int max_allowed_folders, int max_depth, int avg_file_size, int file_size_delta)
            : base(basepath) {
            randomizer = new RandomGenerator(seed);

            _seed = seed;
            _max_allowed_files = max_allowed_files;
            _max_allowed_folders = max_allowed_folders;
            _max_depth = max_depth;
            _avg_file_size = avg_file_size;
            _file_size_delta = file_size_delta;
        }

        public void build() {
            rec_build(basepath);
        }

        public void rec_build(string subpath) {
            int files_to_create = randomizer.Next(_max_allowed_files + 1);
            int folders_to_create = randomizer.Next(_max_allowed_folders + 1);

            for (int file_id = 0; file_id < files_to_create; file_id++)
                create_random_file(subpath);

            _max_allowed_files -= files_to_create;
            _max_allowed_folders -= folders_to_create;

            for (int folder_id = 0; folder_id < folders_to_create; folder_id++) {
                string new_folder_path = create_random_folder(subpath);
                rec_build(new_folder_path);
            }
        }
    }

    class RandomGenerator : System.Random {
        private string allowedchars;

        public RandomGenerator(int seed)
            : base(seed) {
            List<char> forbidden = new List<char>(IO.Path.GetInvalidFileNameChars());
            forbidden.AddRange(IO.Path.GetInvalidPathChars());
            forbidden.Add('.'); //TODO

            Text.StringBuilder builder = new Text.StringBuilder();
            for (int char_id = 0; char_id < (1 << 16); char_id++) {
                char current = (char)char_id;
                if (!forbidden.Contains((char)char_id))
                    builder.Append((char)char_id);
            }
            allowedchars = builder.ToString();
        }

        public char NextPathChar() {
            return "abcdefghijklmnopqrstuvwxyz".ToCharArray()[Next(allowedchars.Length) % 26];
        }

        public string NextFileName() {
            int body_length = 1 + Next(20);
            int ext_length = 1 + Next(8);
            Text.StringBuilder builder = new Text.StringBuilder();

            for (int body_pos = 0; body_pos < body_length; body_pos++)
                builder.Append(NextPathChar());

            builder.Append('.');

            for (int ext_pos = 0; ext_pos < body_length; ext_pos++)
                builder.Append(NextPathChar());

            return builder.ToString();
        }

        public string NextDirectoryName() {
            int body_length = 1 + Next(30);
            Text.StringBuilder builder = new Text.StringBuilder();

            for (int body_pos = 0; body_pos < body_length; body_pos++)
                builder.Append(NextPathChar());

            return builder.ToString();
        }

        public bool NextBool() {
            return Next(2) == 0;
        }

        public IO.FileAttributes NextAttributes(bool is_folder) {
            IO.FileAttributes attr = IO.FileAttributes.Normal;

            if (NextBool()) attr |= IO.FileAttributes.Archive;
            if (NextBool()) attr |= IO.FileAttributes.Compressed;
            if (NextBool()) attr |= IO.FileAttributes.Encrypted;
            if (NextBool()) attr |= IO.FileAttributes.Hidden;
            if (NextBool()) attr |= IO.FileAttributes.NotContentIndexed;
            //if (NextBool()) attr |= IO.FileAttributes.Offline;
            if (NextBool()) attr |= IO.FileAttributes.ReadOnly;
            if (NextBool()) attr |= IO.FileAttributes.SparseFile;
            if (NextBool()) attr |= IO.FileAttributes.System;

            if (is_folder) attr |= IO.FileAttributes.Directory;

            return attr;
        }
    }
}
