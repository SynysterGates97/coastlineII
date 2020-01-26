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

        void DropDuplicates()
        {
            if (_domains != null)
            {
                foreach (var domain in _domains)
                {
                    domain.values = domain.values.Distinct().ToList();
                }
            }
        }

        public List<Domain> GetDomains()
        {
            DropDuplicates();
            return _domains;
        }

        public List<Frame> GetFrames()
        {
            //DropDuplicates();// функция удаления всех дубликатов
            return _frames;
        }

        public void ParseContent()
        {
            LoadContent();
            var json = (JObject)JsonConvert.DeserializeObject(_content);
            var frame = json["Frames"].Value<JArray>();

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
                            // слегка поговнокодил с доменами. Проверь если сможешь

                            bool isNameInDomains = true;

                            parsingFrame.FrameAddSlot(words[0], words[1]);

                            if (_domains != null)
                            {
                                foreach(var domain in _domains)
                                {
                                    if (domain.name == words[0])
                                    {
                                        isNameInDomains = false;                                       
                                        domain.values.Add(words[1]);
                                    }
                                }
                            }

                            if (isNameInDomains)
                            {
                                var newDomain = new Domain();

                                newDomain.name = words[0];
                                newDomain.values.Add(words[1]);

                                _domains.Add(newDomain);
                            }                          
                        }
                    }

                }
                _frames.Add(parsingFrame);
                //MessageBox.Show("Смотри, ПТИЦА!");
            }
        }

        string[] Split(string str)
        {
            string[] words = str.Split(new char[] { ':' });
            return words;
        }

    }
}
