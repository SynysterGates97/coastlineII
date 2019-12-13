using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows;//Удалить

namespace Costaline
{
    class Loader
    {
        string _path;
        string _content;
        List<Domain> _domains;
        private List<Frame> _frames;

        public void SetPath(string path)
        {
            _frames = new List<Frame>();
            _path = path;
        }

        public void LoadContent()
        {
            try {
                var sr = new StreamReader(_path);

                // var task = Task.Run(() => // будет асинхроное чтение из файла / файлов
                //  {
                //    _content = sr.ReadToEnd();
                //});
                _content = sr.ReadToEnd();
            }
            catch {
                
            }
            
        }

        public List<Domain> GetDomains()
        {
            return _domains;
        }

        public List<Frame> GetFrames()
        {
            return _frames;
        }

        public void ParseContent()
        {
            LoadContent();
            var json = (JObject)JsonConvert.DeserializeObject(_content);
            var frame = json["Frames"].Value<JArray>();

<<<<<<< HEAD
            foreach(var f in frame)
                foreach(var str in f)
                {
                    var parsedFrame = new Frame();
=======
            foreach (var f in frame)
            {
                var parseFrame = new Frame();
                foreach (var str in f)
                { 
>>>>>>> c488cd128a6fd830cdd5c8c4d848c45049587932
                    var words = Split(str.ToString());

                    if (words[0] == "name")
                    {
<<<<<<< HEAD
                        parsedFrame.name = words[1];
                        _frames.Add(parsedFrame);
=======
                        parseFrame.name = words[1];
>>>>>>> c488cd128a6fd830cdd5c8c4d848c45049587932
                    }
                    else
                    {
                        if (words[0] == "is_a")
                        {
<<<<<<< HEAD
                            parsedFrame.isA = words[1];
                        }
                        else
                        {
                            parsedFrame.FrameAddSlot(words[0], words[1]);
=======
                            parseFrame.isA = words[1];
                        }
                        else
                        {


                            parseFrame.FrameAddSlot(words[0], words[1]);
>>>>>>> c488cd128a6fd830cdd5c8c4d848c45049587932
                        }
                        _frames.Add(parsedFrame);
                    }
                    
                }
<<<<<<< HEAD
            MessageBox.Show(_frames[0].name);
=======

                _frames.Append(parseFrame);
            }


>>>>>>> c488cd128a6fd830cdd5c8c4d848c45049587932
        }

        string[] Split(string str)
        {
            string[] words = str.Split(new char[] { ':' });

            return words;
        }

    }
}
