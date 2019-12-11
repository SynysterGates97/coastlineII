using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Costaline
{
    class Loader
    {
        string _path;
        string _content;
        List<Domain> _domains;
        List<Frame> _frames;

        public void SetPath(string path)
        {
            _path = path;
        }

        void LoadContent()
        {
            try {
                var sr = new StreamReader(_path);

                var task = Task.Run(() => // будет асинхроное чтение из файла / файлов
                {
                    _content = sr.ReadToEnd();
                });
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

        void ParseContent()
        {
            var json = (JObject)JsonConvert.DeserializeObject(_content);
            var frame = json["Frames"].Value<JArray>();

            foreach(var f in frame)
                foreach(var str in f)
                {
                    var parsFrame = new Frame();
                    var words = Split(str.ToString());

                    if (words[0] == "name")
                    {
                        parsFrame.name = words[1];
                    }
                    else
                    {
                        if (words[0] == "is_a")
                        {
                            parsFrame.isA = words[1];
                        }
                        else
                        {
                            parsFrame.FrameAddSlot(words[0], words[1]);
                        }
                    }
                }
        }

        string[] Split(string str)
        {
            string[] words = str.Split(new char[] { ':' });

            return words;
        }

    }
}
