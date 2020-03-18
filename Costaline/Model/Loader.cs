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

        public Loader()
        {
            _domains = new List<Domain>();
            _frames = new List<Frame>();
        }
        public void SetPath(string path)
        {
            _frames.Clear();
            _domains.Clear();
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

            var domains = json["Domains"].Value<JArray>();

            foreach (var f in frame)
            {
                var parsingFrame = new Frame();
                foreach (var str in f)
                {
                    var words = Split(str.ToString());

                    if (words[0] == "name")
                    {
                        parsingFrame.name = words[1];
                    }
                    else
                    {
                        if (words[0] == "is_a")
                        {
                            parsingFrame.isA = words[1];
                        }
                        else
                        {
                            parsingFrame.FrameAddSlot(words[0], words[1]);                                                  
                        }
                    }

                }
                _frames.Add(parsingFrame);                
            }

            foreach (var d in domains)
            {
                var parsingDomain = new Domain();
                foreach (var str in d)
                {
                    var words = Split(str.ToString());

                    if (words[0] == "name")
                    {
                        parsingDomain.name = words[1];
                    }

                    if (words[0] == "value")
                    {
                        parsingDomain.values.Add(words[1]);
                    }
                }

                _domains.Add(parsingDomain);
            }
        }

        string[] Split(string str)
        {
            string[] words = str.Split(new char[] { ':' });
            return words;
        }

    }
}
