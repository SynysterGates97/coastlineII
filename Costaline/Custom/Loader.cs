using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Costaline
{
    class Loader
    {
        string _path;
        string _content;

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



    }
}
