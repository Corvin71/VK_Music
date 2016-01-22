using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet;
using VkNet.Model.Attachments;

namespace VK_Music.Additional
{
    public class WrapSong //Данный класс идет как обертка для каждой песни
    {
        public Audio _AudioInfo;
        public bool _CheckPlay; //Если false - песня не воспроизводится

        public WrapSong(Audio info)
        {
            _AudioInfo = info;
            _CheckPlay = false;
        }
    }

    public class ListWrapSong
    {
        public ReadOnlyCollection<WrapSong> _ListAudioInfo;

        public ListWrapSong(ReadOnlyCollection<Audio> listIn)
        {
            if (listIn == null) { }
            else
            {
                List<WrapSong> audio = new List<WrapSong>();
                foreach (var temp in listIn)
                {
                    audio.Add(new WrapSong(temp));
                }

                _ListAudioInfo = new ReadOnlyCollection<WrapSong>(audio);
            }
        }

        public bool CheckUrl(Uri url)
        {
            WrapSong returnn = null;
            foreach (var temp in this._ListAudioInfo)
            {
                if (temp._AudioInfo.Url == url)
                {
                    returnn = temp;
                    break;
                }
            }
            if (returnn == null) 
            { 
                return false; 
            }
            else
            {
                return returnn._CheckPlay;
            }
        }

        public void InverseStartPause(Uri url)
        {
            foreach (var temp in this._ListAudioInfo)
            {
                if (temp._AudioInfo.Url == url)
                {
                    temp._CheckPlay = !temp._CheckPlay;
                    break;
                }
            }
        }

        public void ChangeStartPause(Uri url, bool whereat)
        {
            foreach (var temp in this._ListAudioInfo)
            {
                if (temp._AudioInfo.Url == url)
                {
                    temp._CheckPlay = whereat;
                    break;
                }
            }
        }

        public WrapSong ForPopupa(string name)
        {
            WrapSong ret = null;
            foreach (var temp in this._ListAudioInfo)
            {
                if (String.Compare(name, temp._AudioInfo.Title) == 0)
                {
                    ret = temp;
                }
            }
            return ret;
        }

        public int HowMuchMembers()
        {
            int index=0;
            foreach (var temp in this._ListAudioInfo)
            {
                index++;
            }
            return index;
        }

        public WrapSong ReturnNext(Uri uri)
        {
            WrapSong save = null;
            int index = 0;
            foreach (var temp in this._ListAudioInfo)
            {
                if (uri == temp._AudioInfo.Url)
                {
                    save = temp;
                    
                    break;
                }
                index++;
            }

            if (index + 1 < this.HowMuchMembers())
            {
                return this._ListAudioInfo[index + 1];
            }
            else { return null; }
        }
    }
}
