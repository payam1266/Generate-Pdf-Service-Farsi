

//namespace ChopSuey
//{
//    public class MyFontResolver: IFontResolver
//    {
//        public static MyFontResolver Instance { get; private set; }

//        private static readonly Dictionary<string, byte[]> FontData = new Dictionary<string, byte[]>();

//        public MyFontResolver(IWebHostEnvironment env)
//        {
//            // Load font data from files
//            AddFont("Arial Unicode MS", Path.Combine(env.WebRootPath, "fonts", "arial unicode ms regular.ttf"));
//        }

//        private void AddFont(string name, string path)
//        {
//            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
//            {
//                byte[] data = new byte[stream.Length];
//                stream.Read(data, 0, data.Length);
//                FontData[name] = data;
//            }
//        }

//        public byte[] GetFont(string faceName)
//        {
//            if (FontData.TryGetValue(faceName, out var data))
//            {
//                return data;
//            }
//            throw new InvalidOperationException($"Font data for {faceName} not found.");
//        }

//        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
//        {
//            if (familyName.Equals("Arial Unicode MS", StringComparison.OrdinalIgnoreCase))
//            {
//                return new FontResolverInfo("Arial Unicode MS");
//            }
//            return null;
//        }

//        public static void Initialize(IWebHostEnvironment env)
//        {
//            if (Instance == null)
//            {
//                Instance = new MyFontResolver(env);
//            }
//        }
//    }
//}
