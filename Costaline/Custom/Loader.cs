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

            foreach (var f in frame)
            {
                var parseFrame = new Frame();
                foreach (var str in f)
                {
                    var words = Split(str.ToString());

                    if (words[0] == "name")
                    {
                        parseFrame.name = words[1];
                    }
                    else
                    {
                        if (words[0] == "is_a")
                        {
                            parseFrame.isA = words[1];
                        }
                        else
                        {
                            // слегка поговнокодил с доменами. Проверь если сможешь

                            var isNameNotInDonains = false;

                            parseFrame.FrameAddSlot(words[0], words[1]);

                            foreach (var domain in _domains)
                            {
                                if (domain.name == words[0])
                                {
                                    isNameNotInDonains = true;

                                    foreach (var val in domain.values)
                                    {
                                        if (!val.Contains(words[1]))
                                        {
                                            domain.values.Add(words[1]);
                                        }
                                    }
                                }                              
                            }

                            if (!isNameNotInDonains)
                            {
                                var newDom = new Domain();

                                newDom.name = words[0];
                                newDom.values.Add(words[1]);

                                _domains.Add(newDom);
                            }                          
                        }
                    }
                }

                MessageBox.Show(_frames[0].name);
                _frames.Add(parseFrame);
            }
        }

        string[] Split(string str)
        {
            string[] words = str.Split(new char[] { ':' });
            return words;
        }

    }
}
