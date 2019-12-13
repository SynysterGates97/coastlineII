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

        public void ParseContent()
        {
            var json = (JObject)JsonConvert.DeserializeObject(_content);
            var frame = json["Frames"].Value<JArray>();

            foreach(var f in frame)
                foreach(var str in f)
                {
                    var parsedFrame = new Frame();
                    var words = Split(str.ToString());

                    if (words[0] == "name")
                    {
                        parsedFrame.name = words[1];
                        _frames.Add(parsedFrame);
                    }
                    else
                    {
                        if (words[0] == "is_a")
                        {
                            parsedFrame.isA = words[1];
                        }
                        else
                        {
                            parsedFrame.FrameAddSlot(words[0], words[1]);
                        }
                        _frames.Add(parsedFrame);
                    }
                    
                }
            MessageBox.Show(_frames[0].name);
        }

        string[] Split(string str)
        {
            string[] words = str.Split(new char[] { ':' });

            return words;
        }

    }
}
