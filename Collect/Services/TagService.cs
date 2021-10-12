using Collect.Models;
using Newtonsoft.Json;
using Stylet;
using System;
using System.IO;
using System.Linq;
using System.Windows;

namespace Collect.Services
{
    public class TagService
    {
        public static void Import(BindableCollection<Tag> tags, string path)
        {
            if(File.Exists(path))
            {
                string input = "";

                using (StreamReader sr = new StreamReader(path))
                {
                    input = sr.ReadToEnd();
                }

                BindableCollection<Tag> tempTags = JsonConvert.DeserializeObject<BindableCollection<Tag>>(input);

                int dupErrors = 0;
                int formatErrors = 0;

                foreach (Tag tag in tempTags)
                {
                    if (!(tags.Where(x => x.TagId == tag.TagId).ToList().Count > 0))
                    {
                        if ((tag.TagId != null) && (tag.TagDesc != null) && (tag.TraceColor != null))
                            tags.Add(new Tag(tag.TagId, tag.TagDesc, tag.TraceColor));
                        else
                            formatErrors++;
                    }
                    else
                        dupErrors++;
                }

                if ((dupErrors > 0) || (formatErrors > 0))
                    MessageBox.Show(String.Format("{0} tag(s) marked as a duplicate and not imported.\n{1} tag(s) encountered format errors and did not import.",
                        dupErrors, formatErrors), "Import Errors", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public static void Export(BindableCollection<Tag> tags, string path)
        {
            string output = JsonConvert.SerializeObject(tags);

            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(output);
            }
        }
    }
}
