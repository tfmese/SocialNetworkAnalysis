# Graf Algoritmaları ve Sosyal Ağ Analizi Uygulaması

## 1. Proje Bilgileri

**Proje Adı:** Graf Algoritmaları ve Sosyal Ağ Analizi Uygulaması

**Ekip Üyeleri:** [Talha Fırat Meşe - Uğuralp Kıvanç]

**Tarih:** Aralık 2025

**Dil:** C# (.NET 10.0 Windows Forms)

**IDE:** Visual Studio

---


### 2.1 Problemin Tanımı

Sosyal ağlar, iletişim sistemleri gibi sosyal medyadan da aşina olacağımız yapıları modellemek için graf veri yapıları kullanılır. Bu projede, graf üzerinde çeşitli algoritmaların görselleştirilmesi, çalıştırılması ve analiz edilmesi amaçlanmıştır.

### 2.2 Projenin Amacı

Bu proje, aşağıdaki amaçları gerçekleştirmek için geliştirilmiştir:

- Graf veri yapısını görsel olarak oluşturma ve düzenleme
- Pathfinding(Yol Bulma) algoritmalarını (BFS, DFS, Dijkstra, A*) görselleştirme
- Graf analiz algoritmalarını (Degree Centrality(Merkezilik), Connected Components(Bağlı Bileşenler), Welsh-Powell Renklendirme) uygulama
- Algoritmaların performansını ölçme ve karşılaştırma
- Nesne yönelimli programlama(OOP) prensiplerini (Interface, Abstract Class) uygulama
- Graf verilerini CSV ve Komşuluk Matrisi formatında kaydetme ve yükleme

### 2.3 Kullanım Alanları

- Sosyal ağ analizi
- En kısa yol problemleri
- Graf renklendirme problemleri
- İlişki ağları görselleştirme

---

## 3. Algoritmalar

### 3.1 Breadth-First Search (BFS)

#### 3.1.1 Çalışma Mantığı

BFS, graf üzerinde genişlik öncelikli arama yapan bir algoritmadır. FIFO (First In First Out) prensibine göre çalışır ve Queue (kuyruk) veri yapısı kullanır.

**Algoritma Adımları:**
1. Başlangıç düğümü kuyruğa eklenir ve ziyaret edildi olarak işaretlenir
2. Kuyruktan bir düğüm çıkarılır.
3. Bu düğümün tüm komşuları kontrol edilir
4. Ziyaret edilmemiş komşular kuyruğa eklenir
5. Adım 2 ye dönülür, kuyruk boşalana kadar devam edilir

#### 3.1.2 Akış Diyagramı

```mermaid
flowchart TD
    A[Başla] --> B[Başlangıç düğümünü seç]
    B --> C[Başlangıç düğümünü Queue'ya ekle]
    C --> D[Başlangıç düğümünü ziyaret edildi olarak işaretle]
    D --> E{Queue boş mu?}
    E -->|Evet| Z[Bitir]
    E -->|Hayır| F[Queue dan düğüm çıkar]
    F --> G[Düğümü işle]
    G --> H[Düğümün komşularını bul]
    H --> I{Komşu var mı?}
    I -->|Hayır| E
    I -->|Evet| J{Komşu ziyaret edildi mi?}
    J -->|Evet| I
    J -->|Hayır| K[Komşuyu Queue ya ekle]
    K --> L[Komşuyu ziyaret edildi olarak işaretle]
    L --> I
```

#### 3.1.3 Karmaşıklık Analizi

- **Zaman Karmaşıklığı:** O(V + E)
  - V: Düğüm sayısı (Vertex)
  - E: Kenar sayısı (Edge)
  - Her düğüm bir kez ziyaret edilir, her kenar bir kez kontrol edilir

- **Uzay Karmaşıklığı:** O(V)
  - Queue'da en fazla V düğüm bulunabilir
  - Ziyaret edilen düğümler için O(V) bellek

#### 3.1.4 Literatür İncelemesi

BFS algoritması ilk olarak 1959 yılında Edward F. Moore tarafından labirent çözme problemi için önerilmiştir. Daha sonra C.Y. Lee tarafından 1961'de bağlantı yolları bulma problemi için uygulanmıştır. BFS, en kısa yol problemi için ağırlıksız graflarda optimal çözüm sağlar.

**Kaynak:** Moore, E. F. (1959). "The shortest path through a maze". Proceedings of the International Symposium on the Theory of Switching.

---

### 3.2 Depth-First Search (DFS)

#### 3.2.1 Çalışma Mantığı

DFS, graf üzerinde derinlik öncelikli arama yapan bir algoritmadır. LIFO (Last In First Out) prensibine göre çalışır ve Stack (yığın) veri yapısı kullanır.

**Algoritma Adımları:**
1. Başlangıç düğümü stack'e eklenir
2. Stack'ten bir düğüm çıkarılır
3. Eğer düğüm ziyaret edilmemişse, ziyaret edildi olarak işaretlenir
4. Bu düğümün ziyaret edilmemiş komşuları stack'e eklenir
5. Adım 2'ye dönülür, stack boşalana kadar devam edilir

#### 3.2.2 Akış Diyagramı

```mermaid
flowchart TD
    A[Başla] --> B[Başlangıç düğümünü seç]
    B --> C[Başlangıç düğümünü Stack'e ekle]
    C --> D{Stack boş mu?}
    D -->|Evet| Z[Bitir]
    D -->|Hayır| E[Stack'ten düğüm çıkar]
    E --> F{Düğüm ziyaret edildi mi?}
    F -->|Evet| D
    F -->|Hayır| G[Düğümü ziyaret edildi olarak işaretle]
    G --> H[Düğümü işle]
    H --> I[Düğümün komşularını bul]
    I --> J{Komşu var mı?}
    J -->|Hayır| D
    J -->|Evet| K{Komşu ziyaret edildi mi?}
    K -->|Evet| J
    K -->|Hayır| L[Komşuyu Stack'e ekle]
    L --> J
```
#### 3.2.3 Karmaşıklık Analizi

- **Zaman Karmaşıklığı:** O(V + E)
  - V: Düğüm sayısı
  - E: Kenar sayısı
  - Her düğüm bir kez ziyaret edilir

- **Uzay Karmaşıklığı:** O(V)
  - Stack'te en fazla V düğüm bulunabilir (en kötü durum: lineer graf)
  - Ziyaret edilen düğümler için O(V) bellek

#### 3.2.4 Literatür İncelemesi

DFS algoritması, Charles Pierre Trémaux tarafından 19. yüzyılda labirent çözme problemi için önerilmiştir. Algoritma, topolojik sıralama, strongly connected components bulma ve cycle detection gibi problemlerde yaygın olarak kullanılır.

**Kaynak:** Tarjan, R. (1972). "Depth-first search and linear graph algorithms". SIAM Journal on Computing.

---

### 3.3 Dijkstra Algoritması

#### 3.3.1 Çalışma Mantığı

Dijkstra algoritması, ağırlıklı graflarda bir başlangıç düğümünden diğer tüm düğümlere olan en kısa yolları bulur. Greedy (açgözlü) algoritma yaklaşımı kullanır.

**Algoritma Adımları:**
1. Tüm düğümlerin mesafesi sonsuz olarak başlatılır (başlangıç düğümü 0)
2. Ziyaret edilmemiş düğümler arasından en kısa mesafeye sahip olan seçilir
3. Seçilen düğümün komşuları kontrol edilir
4. Eğer daha kısa bir yol bulunursa, mesafe güncellenir
5. Seçilen düğüm ziyaret edildi olarak işaretlenir
6. Adım 2'ye dönülür, tüm düğümler ziyaret edilene kadar devam edilir

#### 3.3.2 Akış Diyagramı

```mermaid
flowchart TD
    A[Başla] --> B[Tüm düğümlerin mesafesini sonsuz yap]
    B --> C[Başlangıç düğümünün mesafesini 0 yap]
    C --> D{Ziyaret edilmemiş düğüm var mı?}
    D -->|Hayır| Z[Bitir]
    D -->|Evet| E[En kısa mesafeye sahip düğümü seç]
    E --> F[Seçilen düğümü ziyaret edildi olarak işaretle]
    F --> G[Seçilen düğümün komşularını bul]
    G --> H{Komşu var mı?}
    H -->|Hayır| D
    H -->|Evet| I{Komşu ziyaret edildi mi?}
    I -->|Evet| H
    I -->|Hayır| J[Yeni mesafe hesapla: mevcut mesafe + kenar ağırlığı]
    J --> K{Yeni mesafe daha kısa mı?}
    K -->|Hayır| H
    K -->|Evet| L[Mesafeyi güncelle]
    L --> M[Önceki düğümü kaydet]
    M --> H
```

#### 3.3.3 Karmaşıklık Analizi

- **Zaman Karmaşıklığı:** O(V²)
  - Basit implementasyon: O(V²) - her düğüm için tüm düğümleri kontrol eder
  - Priority Queue kullanılırsa: O((V + E) log V)
  - Bu projede basit implementasyon kullanıldı: O(V²)

- **Uzay Karmaşıklığı:** O(V)
  - Mesafe dizisi: O(V)
  - Ziyaret edilen düğümler: O(V)
  - Önceki düğümler: O(V)

#### 3.3.4 Literatür İncelemesi

Dijkstra algoritması, 1956 yılında Edsger W. Dijkstra tarafından Amsterdam'daki bir bilgisayar için en kısa yol problemi çözümü olarak geliştirilmiştir. Algoritma, negatif ağırlıklı kenarlar içermeyen graflarda optimal çözüm sağlar.

**Kaynak:** Dijkstra, E. W. (1959). "A note on two problems in connexion with graphs". Numerische Mathematik.

---

### 3.4 A* (A-Star) Algoritması

#### 3.4.1 Çalışma Mantığı

A* algoritması, Dijkstra algoritmasının geliştirilmiş bir versiyonudur. Heuristic fonksiyon kullanarak daha hızlı sonuç verir. Formül: **f(n) = g(n) + h(n)**
- **g(n):** Başlangıçtan n düğümüne kadar olan gerçek maliyet
- **h(n):** n düğümünden hedefe kadar olan tahmini maliyet (heuristic)
- **f(n):** Toplam tahmini maliyet

**Algoritma Adımları:**
1. Başlangıç düğümü open set'e eklenir (g=0, f=h)
2. Open set'ten en düşük f değerine sahip düğüm seçilir
3. Seçilen düğümün komşuları kontrol edilir
4. Her komşu için g ve f değerleri hesaplanır
5. Daha iyi bir yol bulunursa güncellenir
6. Hedefe ulaşıldığında durulur

#### 3.4.2 Akış Diyagramı

```mermaid
flowchart TD
    A[Başla] --> B[Başlangıç düğümünü Open Set'e ekle]
    B --> C[gScore başlangıç = 0]
    C --> D[fScore başlangıç = Heuristic başlangıç, hedef]
    D --> E{Open Set boş mu?}
    E -->|Evet| Z1[Hedefe ulaşılamadı]
    E -->|Hayır| F[En düşük fScore'a sahip düğümü seç]
    F --> G{Seçilen düğüm hedef mi?}
    G -->|Evet| Z2[Yol bulundu - Bitir]
    G -->|Hayır| H[Seçilen düğümü Open Set'ten çıkar]
    H --> I[Seçilen düğümün komşularını bul]
    I --> J{Komşu var mı?}
    J -->|Hayır| E
    J -->|Evet| K[Kenar ağırlığını bul]
    K --> L[tentative_gScore = gScore mevcut + kenar ağırlığı]
    L --> M{tentative_gScore < gScore komşu?}
    M -->|Hayır| J
    M -->|Evet| N[gScore komşu = tentative_gScore]
    N --> O[fScore komşu = gScore komşu + Heuristic komşu, hedef]
    O --> P{Komşu Open Set'te mi?}
    P -->|Hayır| Q[Komşuyu Open Set'e ekle]
    Q --> R[previousNodes komşu = mevcut düğüm]
    R --> J
    P -->|Evet| R
```

#### 3.4.3 Karmaşıklık Analizi

- **Zaman Karmaşıklığı:** O(b^d)
  - b: Branching factor (ortalama komşu sayısı)
  - d: Çözüm derinliği
  - Pratikte Dijkstra'dan daha hızlıdır çünkü hedefe yönelik arama yapar

- **Uzay Karmaşıklığı:** O(b^d)
  - Open set'te saklanan düğüm sayısı
  - Heuristic fonksiyon kalitesine bağlı olarak değişir

#### 3.4.4 Literatür İncelemesi

A* algoritması, 1968 yılında Peter Hart, Nils Nilsson ve Bertram Raphael tarafından geliştirilmiştir. Algoritma, admissible (kabul edilebilir) heuristic fonksiyon kullanıldığında optimal çözüm sağlar. Oyun geliştirme, robotik ve navigasyon sistemlerinde yaygın olarak kullanılır.

**Kaynak:** Hart, P. E., Nilsson, N. J., & Raphael, B. (1968). "A Formal Basis for the Heuristic Determination of Minimum Cost Paths". IEEE Transactions on Systems Science and Cybernetics.

---

### 3.5 Degree Centrality (Merkezilik)

#### 3.5.1 Çalışma Mantığı

Merkezilik, bir düğümün graf içindeki önemini, o düğüme bağlı olan kenar sayısına göre ölçer. Sosyal ağlarda daha fazla bağlantıya sahip kullanıcılar daha merkezi kabul edilir.

**Formül:**
```
Degree Centrality = (Düğümün derecesi) / (Toplam düğüm sayısı - 1)
```

**Algoritma Adımları:**
1. Her düğüm için bağlantı sayısı (degree) hesaplanır
2. Düğümler dereceye göre sıralanır
3. En yüksek dereceye sahip ilk 5 düğüm seçilir
4. Merkezilik skoru hesaplanır ve gösterilir

#### 3.5.2 Akış Diyagramı

```mermaid
flowchart TD
    A[Başla] --> B[Her düğüm için döngü başlat]
    B --> C[Düğümün bağlı olduğu kenarları say]
    C --> D[Derece = kenar sayısı]
    D --> E{Diğer düğümler var mı?}
    E -->|Evet| B
    E -->|Hayır| F[Tüm düğümleri dereceye göre sırala]
    F --> G[İlk 5 düğümü seç]
    G --> H[Her düğüm için merkezilik skoru hesapla]
    H --> I[Sonuçları tabloya yazdır]
    I --> Z[Bitir]
```

#### 3.5.3 Karmaşıklık Analizi

- **Zaman Karmaşıklığı:** O(V + E)
  - Her kenar bir kez kontrol edilir: O(E)
  - Sıralama: O(V log V)
  - Toplam: O(V + E) (E genellikle V'den büyüktür)

- **Uzay Karmaşıklığı:** O(V)
  - Düğüm derecelerini saklamak için

#### 3.5.4 Literatür İncelemesi

Merkezilik, sosyal ağ analizinde en temel ölçütlerden biridir. Linton Freeman tarafından 1979'da formalize edilmiştir. Sosyal medya platformlarında influencer tespiti, ağ güvenliği analizi ve bilgi yayılımı çalışmalarında kullanılır.

**Kaynak:** Freeman, L. C. (1979). "Centrality in social networks conceptual clarification". Social Networks.

---

### 3.6 Connected Components (Bağlı Bileşenler)

#### 3.6.1 Çalışma Mantığı

Connected Components algoritması, graf içindeki birbirine bağlı düğüm gruplarını bulur. İki düğüm arasında yol varsa, bağlı yani aynı grupta kabul edilirler.

**Algoritma Adımları:**
1. Tüm düğümler ziyaret edilmemiş olarak işaretlenir
2. Ziyaret edilmemiş bir düğüm seçilir
3. Bu düğümden başlayarak BFS ile ulaşılabilen tüm düğümler bulunur
4. Bulunan düğümler bir bileşen olarak kaydedilir
5. Adım 2'ye dönülür, tüm düğümler ziyaret edilene kadar devam edilir

#### 3.6.2 Akış Diyagramı

```mermaid
flowchart TD
    A[Başla] --> B[Tüm düğümleri ziyaret edilmemiş olarak işaretle]
    B --> C{Ziyaret edilmemiş düğüm var mı?}
    C -->|Hayır| Z[Bitir]
    C -->|Evet| D[Yeni bir bileşen başlat]
    D --> E[Ziyaret edilmemiş bir düğüm seç]
    E --> F[Düğümü ziyaret edildi olarak işaretle]
    F --> G[Düğümü bileşene ekle]
    G --> H[Düğümü Queue'ya ekle]
    H --> I{Queue boş mu?}
    I -->|Evet| C
    I -->|Hayır| J[Queue'dan düğüm çıkar]
    J --> K[Düğümün komşularını bul]
    K --> L{Komşu var mı?}
    L -->|Hayır| I
    L -->|Evet| M{Komşu ziyaret edildi mi?}
    M -->|Evet| L
    M -->|Hayır| N[Komşuyu ziyaret edildi olarak işaretle]
    N --> O[Komşuyu bileşene ekle]
    O --> P[Komşuyu Queue ya ekle]
    P --> L
```

#### 3.6.3 Karmaşıklık Analizi

- **Zaman Karmaşıklığı:** O(V + E)
  - Her düğüm bir kez ziyaret edilir: O(V)
  - Her kenar bir kez kontrol edilir: O(E)
  - Toplam: O(V + E)

- **Uzay Karmaşıklığı:** O(V)
  - Queue için: O(V)
  - Ziyaret edilen düğümler: O(V)
  - Bileşen listesi: O(V)

#### 3.6.4 Literatür İncelemesi

Connected Components problemi, graf teorisinin temel problemlerinden biridir. Tarjan'ın algoritması (1972) ve Union-Find veri yapısı kullanılarak çözülebilir. Ağ analizi, sosyal ağ topluluk tespiti ve bilgisayar ağları güvenliği alanlarında kullanılır.

**Kaynak:** Tarjan, R. (1972). "Depth-first search and linear graph algorithms". SIAM Journal on Computing.

---

### 3.7 Welsh-Powell Graf Renklendirme Algoritması

#### 3.7.1 Çalışma Mantığı

Welsh-Powell algoritması, bir grafın düğümlerini, komşu düğümleri farkklı renkte olacak şekilde renklerndirir. 

**Algoritma Adımları:**
1. Önce ayrık topluluklar (connected components) bulunur
2. Her topluluk için ayrı ayrı renklendirme yapılır
3. Düğümler derecelerine göre azalan sırada sıralanır
4. Sırayla her düğüm kontrol edilir
5. Eğer düğümün komşuları bu renge sahip değilse, düğüme bu renk atanır
6. Renk atanamayan düğümler için yeni renk oluşturulur
7. Tüm düğümler renklendirilene kadar devam edilir

#### 3.7.2 Akış Diyagramı

```mermaid
flowchart TD
    A[Başla] --> B[Ayrık toplulukları bul]
    B --> C{Topluluk var mı?}
    C -->|Hayır| Z[Bitir]
    C -->|Evet| D[Topluluk için döngü başlat]
    D --> E[Düğümleri dereceye göre azalan sırada sırala]
    E --> F{Renklendirilmemiş düğüm var mı?}
    F -->|Hayır| C
    F -->|Evet| G[Renk sayacı = 0]
    G --> H[Sıradaki renklendirilmemiş düğümü seç]
    H --> I{Düğümün komşuları bu renge sahip mi?}
    I -->|Evet| J{Başka renklendirilmemiş düğüm var mı?}
    J -->|Evet| H
    J -->|Hayır| K[Renk sayacını artır]
    K --> L{Yeni renk var mı?}
    L -->|Hayır| F
    L -->|Evet| G
    I -->|Hayır| M[Düğüme bu rengi ata]
    M --> N{Düğüm renklendirildi mi?}
    N -->|Evet| J
    N -->|Hayır| K
```

#### 3.7.3 Karmaşıklık Analizi

- **Zaman Karmaşıklığı:** O(V² +E)
  - Connected Components bulma: O(V+ E)
  - Sıralama: O(VlogV)
  - Her düğüm için komşu kontrolü: O(V + E)
  - Toplam: O(V² +E)

- **Uzay Karmaşıklığı:** O(V)
  - Renk atamalarını saklamak için
  - Topluluk listesi: O(V)

#### 3.7.4 Literatür İncelemesi

Welsh-Powell algoritması, 1967 yılında D.J.A. Welsh ve M.B. Powell tarafından önerilmiştir. Graf renklendirme problemi, zamanlama problemleri, kaynak tahsisi ve register allocation gibi birçok uygulamada kullanılır.

**Kaynak:** Welsh, D. J. A., & Powell, M. B. (1967). "An upper bound for the chromatic number of a graph and its application to timetabling problems". The Computer Journal.

---

## 4. Proje Yapısı ve Sınıf Diyagramları

### 4.1 Genel Sınıf Yapısı

```mermaid
classDiagram
    class Graph {
        +List~Node~ Nodes
        +List~Edge~ Edges
        +AddNode(Node)
        +AddEdge(Node, Node)
    }
    
    class Node {
        +int Id
        +string Name
        +float X
        +float Y
        +double Activity
        +double Interaction
        +double ConnectionCount
        +bool Visited
        +Color CurrentColor
    }
    
    class Edge {
        +Node Source
        +Node Target
        +double Weight
        +Color Color
        +int Thickness
        +CalculateWeight() double
    }
    
    class IGraphAlgorithm {
        <<interface>>
        +ExecuteAsync(Graph, Node, Node, Panel, Label) Task
    }
    
    class IGraphAnalyzer {
        <<interface>>
        +Analyze(Graph, object) void
    }
    
    class IFileHandler {
        <<interface>>
        +LoadGraphFromCSV(string, int, int) Graph
        +SaveGraphToCSV(Graph, string) bool
        +SaveAdjacencyMatrix(Graph, string) void
    }
    
    class AbstractPathfindingAlgorithm {
        <<abstract>>
        #GetNeighbors(Graph, Node) List~Node~
        #ResetGraph(Graph) void
        #FindEdge(Graph, Node, Node) Edge
        #CalculateHeuristic(Node, Node) double
        +ExecuteAsync(...)* Task
    }
    
    class AbstractGraphAnalyzer {
        <<abstract>>
        #GetNeighbors(Graph, Node) List~Node~
        #ResetGraph(Graph) void
        +Analyze(...)* void
    }
    
    class BFSAlgorithm {
        +ExecuteAsync(...) Task
    }
    
    class DFSAlgorithm {
        +ExecuteAsync(...) Task
    }
    
    class DijkstraAlgorithm {
        +ExecuteAsync(...) Task
    }
    
    class AStarAlgorithm {
        +ExecuteAsync(...) Task
    }
    
    class DegreeCentralityAnalyzer {
        +Analyze(...) void
    }
    
    class ConnectedComponentsAnalyzer {
        +Analyze(...) void
    }
    
    class WelshPowellColoringAnalyzer {
        +Analyze(...) void
    }
    
    class ColoringResult {
        +List~Node~ Component
        +Dictionary~Node,int~ NodeColors
        +int ColorCount
    }
    
    class FileManager {
        +LoadGraphFromCSV(...) Graph
        +SaveGraphToCSV(...) bool
        +SaveAdjacencyMatrix(...) void
    }
    
    class Algorithms {
        -IGraphAlgorithm bfsAlgo
        -IGraphAlgorithm dfsAlgo
        -IGraphAlgorithm dijkstraAlgo
        -IGraphAlgorithm aStarAlgo
        -IGraphAnalyzer centralityAnalyzer
        -IGraphAnalyzer componentsAnalyzer
        -IGraphAnalyzer coloringAnalyzer
        +RunBFS(...) Task
        +RunDFS(...) Task
        +RunDijkstra(...) Task
        +RunAStar(...) Task
        +CalculateDegreeCentrality(...) void
        +GetConnectedComponents(...) List~List~Node~~
        +RunWelshPowellColoring(...) List~ColoringResult~
    }
    
    class Form1 {
        -Graph socialGraph
        -Node selectedNode
        -Node startNode
        -Node endNode
        +pnlGraph_Paint(...) void
        +pnlGraph_MouseClick(...) void
        +btnRunBFS_Click(...) void
        +btnRunDFS_Click(...) void
        +btn_Dijkstra_Click(...) void
        +btn_AStar_Click(...) void
    }
    
    Graph "1" *-- "many" Node
    Graph "1" *-- "many" Edge
    Edge "1" --> "1" Node : Source
    Edge "1" --> "1" Node : Target
    
    IGraphAlgorithm <|.. AbstractPathfindingAlgorithm
    AbstractPathfindingAlgorithm <|-- BFSAlgorithm
    AbstractPathfindingAlgorithm <|-- DFSAlgorithm
    AbstractPathfindingAlgorithm <|-- DijkstraAlgorithm
    AbstractPathfindingAlgorithm <|-- AStarAlgorithm
    
    IGraphAnalyzer <|.. AbstractGraphAnalyzer
    AbstractGraphAnalyzer <|-- DegreeCentralityAnalyzer
    AbstractGraphAnalyzer <|-- ConnectedComponentsAnalyzer
    AbstractGraphAnalyzer <|-- WelshPowellColoringAnalyzer
    
    IFileHandler <|.. FileManager
    
    Algorithms --> IGraphAlgorithm
    Algorithms --> IGraphAnalyzer
    Form1 --> Algorithms
    Form1 --> IFileHandler
    Form1 --> Graph
    
    WelshPowellColoringAnalyzer --> ColoringResult
```

### 4.2 Interface ve Abstract Class İlişkileri

```mermaid
graph TB
    subgraph Interfaces
        I1[IGraphAlgorithm]
        I2[IGraphAnalyzer]
        I3[IFileHandler]
    end
    
    subgraph Abstract Classes
        A1[AbstractPathfindingAlgorithm]
        A2[AbstractGraphAnalyzer]
    end
    
    subgraph Pathfinding Algorithms
        P1[BFSAlgorithm]
        P2[DFSAlgorithm]
        P3[DijkstraAlgorithm]
        P4[AStarAlgorithm]
    end
    
    subgraph Analysis Algorithms
        AN1[DegreeCentralityAnalyzer]
        AN2[ConnectedComponentsAnalyzer]
        AN3[WelshPowellColoringAnalyzer]
    end
    
    subgraph File Operations
        F1[FileManager]
    end
    
    I1 -->|implements| A1
    A1 -->|extends| P1
    A1 -->|extends| P2
    A1 -->|extends| P3
    A1 -->|extends| P4
    
    I2 -->|implements| A2
    A2 -->|extends| AN1
    A2 -->|extends| AN2
    A2 -->|extends| AN3
    
    I3 -->|implements| F1
### 4.3 Sistem Mimarisi

```mermaid
graph TB
    subgraph "Kullanıcı Arayüzü Katmanı"
        UI[Form1 - Windows Forms]
    end
    
    subgraph "İş Mantığı Katmanı"
        FACADE[Algorithms - Facade Pattern]
    end
    
    subgraph "Algoritma Katmanı"
        PATH[Pathfinding Algorithms<br/>BFS, DFS, Dijkstra, A*]
        ANALYZE[Analysis Algorithms<br/>Centrality, Components, Coloring]
    end
    
    subgraph "Veri Yapıları Katmanı"
        GRAPH[Graph]
        NODE[Node]
        EDGE[Edge]
    end
    
    subgraph "Dosya İşlemleri Katmanı"
        FILE[FileManager]
    end
    
    UI --> FACADE
    FACADE --> PATH
    FACADE --> ANALYZE
    PATH --> GRAPH
    ANALYZE --> GRAPH
    GRAPH --> NODE
    GRAPH --> EDGE
    UI --> FILE
    FILE --> GRAPH
```

### 4.4 Sequence Diyagramı - Algoritma Çalıştırma

```mermaid
sequenceDiagram
    participant User as Kullanıcı
    participant Form1 as Form1
    participant Algorithms as Algorithms Facade
    participant Interface as IGraphAlgorithm
    participant Algo as Concrete Algorithm
    participant Graph as Graph
    participant Panel as Drawing Panel
    
    User->>Form1: Butona Tıkla (BFS/DFS/Dijkstra/A*)
    Form1->>Algorithms: RunBFS/RunDFS/RunDijkstra/RunAStar()
    Algorithms->>Interface: ExecuteAsync()
    Interface->>Algo: ExecuteAsync() (Polymorphism)
    Algo->>Graph: GetNeighbors()
    Graph-->>Algo: Neighbors List
    Algo->>Algo: Algoritma Mantığı
    Algo->>Panel: Invalidate() (Görselleştirme)
    Algo->>Form1: Update Time Label
    Form1-->>User: Sonuç Göster
```

### 4.5 State Diyagramı - Graf Durumları

```mermaid
stateDiagram-v2
    [*] --> BoşGraf: Uygulama Başlat
    
    BoşGraf --> DüğümEkleme: Kullanıcı Tıklar
    DüğümEkleme --> GrafOluşturuldu: Düğüm Eklendi
    
    GrafOluşturuldu --> KenarEkleme: İki Düğüm Seçildi
    KenarEkleme --> GrafOluşturuldu: Kenar Eklendi
    
    GrafOluşturuldu --> AlgoritmaÇalışıyor: Algoritma Başlatıldı
    AlgoritmaÇalışıyor --> GrafOluşturuldu: Algoritma Bitti
    
    GrafOluşturuldu --> AnalizYapılıyor: Analiz Başlatıldı
    AnalizYapılıyor --> GrafOluşturuldu: Analiz Bitti
    
    GrafOluşturuldu --> DosyaKaydediliyor: Kaydet Butonu
    DosyaKaydediliyor --> GrafOluşturuldu: Dosya Kaydedildi
    
    GrafOluşturuldu --> DosyaYükleniyor: Yükle Butonu
    DosyaYükleniyor --> GrafOluşturuldu: Dosya Yüklendi
    
    GrafOluşturuldu --> BoşGraf: Reset Butonu
```

### 4.6 Veri Akış Diyagramı

```mermaid
flowchart LR
    subgraph "Girdi"
        CSV[CSV Dosyası]
        MOUSE[Mouse Etkileşimi]
    end
    
    subgraph "İşleme"
        LOAD[FileManager<br/>Yükleme]
        CREATE[Graf Oluşturma]
        ALGO[Algoritma İşleme]
        ANALYZE[Analiz İşleme]
    end
    
    subgraph "Veri Yapısı"
        GRAPH[(Graph)]
        NODES[(Nodes)]
        EDGES[(Edges)]
    end
    
    subgraph "Çıktı"
        VISUAL[Görselleştirme]
        RESULT[Sonuç Tabloları]
        FILEOUT[Kaydedilmiş Dosya]
    end
    
    CSV --> LOAD
    MOUSE --> CREATE
    LOAD --> GRAPH
    CREATE --> GRAPH
    GRAPH --> NODES
    GRAPH --> EDGES
    GRAPH --> ALGO
    GRAPH --> ANALYZE
    ALGO --> VISUAL
    ANALYZE --> RESULT
    GRAPH --> FILEOUT
```

### 4.7 Modül Yapısı ve İşlevleri

Proje, modüler bir mimari kullanılarak organize edilmiştir. Her modül kendi sorumluluğuna odaklanarak kodun bakımını ve genişletilebilirliğini kolaylaştırmaktadır.

#### 4.7.1 Models Modülü (Veri Yapıları)

**Graph.cs**
- Graf veri yapısını temsil eder
- Düğüm ve kenar listelerini tutar (`List<Node> Nodes`, `List<Edge> Edges`)
- Düğüm ekleme metodu: `AddNode(Node node)`
- Kenar ekleme metodu: `AddEdge(Node source, Node target)` - self-loop kontrolü yapar
- Namespace: `ProjectYazLab.Models`

**Node.cs**
- Graf düğümlerini temsil eder
- Temel özellikler:
  - `Id`: Düğüm kimliği
  - `Name`: Düğüm adı
  - `X`, `Y`: Koordinat bilgileri (görselleştirme için)
- Sosyal ağ özellikleri:
  - `Activity`: Aktiflik değeri
  - `Interaction`: Etkileşim değeri
  - `ConnectionCount`: Bağlantı sayısı
- Görselleştirme özellikleri:
  - `CurrentColor`: Düğümün mevcut rengi (varsayılan: Mavi)
  - `Visited`: Ziyaret durumu (algoritmalar için)

**Edge.cs**
- Graf kenarlarını temsil eder
- Özellikler:
  - `Source`: Kaynak düğüm
  - `Target`: Hedef düğüm
  - `Weight`: Kenar ağırlığı (otomatik hesaplanır)
  - `Color`: Görselleştirme rengi (varsayılan: Siyah)
  - `Thickness`: Çizgi kalınlığı (varsayılan: 2)
- Ağırlık hesaplama metodu: `CalculateWeight()`
- Formül: `Weight = 1 + √[(Activity_i - Activity_j)² + (Interaction_i - Interaction_j)² + (ConnectionCount_i - ConnectionCount_j)²]`
- Ağırlık, düğümler arasındaki benzerlik farkına göre hesaplanır

**ColoringResult.cs**
- Welsh-Powell renklendirme algoritmasının sonuçlarını tutan veri yapısı
- Özellikler:
  - `Component`: Renklendirilen topluluk (düğüm listesi)
  - `NodeColors`: Her düğümün atanan renk indeksini tutan dictionary (`Dictionary<Node, int>`)
  - `ColorCount`: Toplulukta kullanılan toplam renk sayısı
- Her ayrık topluluk için ayrı bir `ColoringResult` nesnesi oluşturulur
- Renklendirme sonuçlarının görselleştirilmesi ve raporlanması için kullanılır

#### 4.7.2 Interfaces Modülü (Arayüzler)

**IGraphAlgorithm.cs**
- Pathfinding algoritmaları için arayüz
- Metod: `Task ExecuteAsync(Graph graph, Node startNode, Node endNode, Panel drawingPanel, Label timeLabel)`
- Tüm pathfinding algoritmaları (BFS, DFS, Dijkstra, A*) bu arayüzü implement eder
- Asenkron çalışma desteği sağlar

**IGraphAnalyzer.cs**
- Graf analiz algoritmaları için arayüz
- Metod: `void Analyze(Graph graph, object resultContainer)`
- Analiz algoritmaları (Degree Centrality, Connected Components, Welsh-Powell Coloring) bu arayüzü implement eder
- Sonuçlar `resultContainer` parametresi üzerinden döndürülür

**IFileHandler.cs**
- Dosya işlemleri için arayüz
- Metodlar:
  - `Graph LoadGraphFromCSV(string filePath, int maxWidth, int maxHeight)`: CSV'den graf yükleme
  - `bool SaveGraphToCSV(Graph graph, string filePath)`: Grafi CSV formatında kaydetme
  - `void SaveAdjacencyMatrix(Graph graph, string filePath)`: Komşuluk matrisi formatında kaydetme
- Dosya işlemlerinin soyutlanmasını sağlar

#### 4.7.3 AlgoModule Modülü (Algoritma Implementasyonları)

**AbstractPathfindingAlgorithm.cs**
- Pathfinding algoritmaları için abstract base class
- Ortak fonksiyonellik sağlar:
  - `GetNeighbors(Graph graph, Node node)`: Bir düğümün komşularını bulur (yönsüz graf için iki yönlü kontrol)
  - `ResetGraph(Graph graph)`: Tüm düğümleri varsayılan duruma sıfırlar (Visited = false, CurrentColor = Blue)
  - `FindEdge(Graph graph, Node source, Node target)`: İki düğüm arasındaki kenarı bulur
  - `CalculateHeuristic(Node a, Node b)`: İki düğüm arası Öklid mesafesi hesaplar (A* algoritması için)
- `IGraphAlgorithm` interface'ini implement eder
- Namespace: `ProjectYazLab.AlgoModule`

**BFSAlgorithm.cs**
- Breadth-First Search (Genişlik Öncelikli Arama) algoritması
- Queue (Kuyruk) veri yapısı kullanır (FIFO prensibi)
- Başlangıç düğümünden itibaren tüm komşuları sırayla ziyaret eder
- Halka halka (level by level) tarama yapar
- Görselleştirme: Turuncu (ziyaret edilecek), Açık Yeşil (ziyaret edildi)

**DFSAlgorithm.cs**
- Depth-First Search (Derinlik Öncelikli Arama) algoritması
- Stack (Yığın) veri yapısı kullanır (LIFO prensibi)
- Bir düğümden başlayarak mümkün olduğunca derine iner, sonra geri döner (backtracking)
- Görselleştirme: Turuncu renk ile ziyaret sırası gösterilir

**DijkstraAlgorithm.cs**
- En kısa yol bulma algoritması
- Ağırlıklı graflarda başlangıç düğümünden hedef düğüme en kısa yolu bulur
- Her düğüm için minimum mesafeyi hesaplar
- Greedy yaklaşım kullanır
- Görselleştirme: Bulunan yol Mor renkle gösterilir
- Toplam maliyet bilgisi kullanıcıya gösterilir

**AStarAlgorithm.cs**
- A* (A-Star) pathfinding algoritması
- Dijkstra'dan farklı olarak heuristic fonksiyon kullanır
- Formül: `f(n) = g(n) + h(n)`
  - `g(n)`: Başlangıçtan n düğümüne gerçek maliyet
  - `h(n)`: n düğümünden hedefe tahmini mesafe (Öklid mesafesi)
- Daha verimli yol bulma sağlar
- Görselleştirme: Bulunan yol Mor renkle gösterilir

**AbstractGraphAnalyzer.cs**
- Graf analiz algoritmaları için abstract base class
- Ortak fonksiyonellik:
  - `GetNeighbors(Graph graph, Node node)`: Komşu düğümleri bulur
  - `ResetGraph(Graph graph)`: Grafi sıfırlar
- `IGraphAnalyzer` interface'ini implement eder

**DegreeCentralityAnalyzer.cs**
- Degree Centrality (Derece Merkezilik) analizi
- Her düğümün bağlantı sayısını (degree) hesaplar
- En yüksek dereceli 5 düğümü listeler
- Merkezilik skoru: `Score = Degree / (NodeCount - 1)`
- Sonuçlar DataGridView'de gösterilir

**ConnectedComponentsAnalyzer.cs**
- Bağlı bileşenleri (Connected Components) bulma
- BFS algoritması kullanarak ayrık toplulukları tespit eder
- Her topluluk için düğüm listesi döndürür
- Sonuç: `List<List<Node>>` formatında

**WelshPowellColoringAnalyzer.cs**
- Welsh-Powell graf renklendirme algoritması
- Her ayrık topluluk için ayrı ayrı renklendirme yapar
- Algoritma adımları:
  1. Düğümleri derecelerine göre azalan sırada sıralar
  2. Her düğüm için komşularında kullanılmayan en küçük rengi atar
  3. Minimum renk sayısı ile renklendirme yapar
- 20 farklı renk paleti kullanır
- Sonuç: `List<ColoringResult>` formatında döner
- Her topluluk için renk sayısı ve düğüm-renk eşleşmeleri tutulur

#### 4.7.4 Services Modülü (Servis Sınıfları)

**FileManager.cs**
- Dosya işlemlerini yöneten servis sınıfı
- `IFileHandler` interface'ini implement eder
- Fonksiyonellik:
  - **CSV Yükleme**: CSV dosyasından graf verilerini okur, Node ve Edge nesneleri oluşturur
    - Koordinat bilgisi yoksa rastgele koordinat atar
    - Komşuluk bilgilerini parse ederek kenarları oluşturur
  - **CSV Kaydetme**: Graf verilerini CSV formatında kaydeder
    - Tüm düğüm bilgileri ve komşuluk ilişkileri kaydedilir
  - **Komşuluk Matrisi Kaydetme**: Grafı komşuluk matrisi formatında kaydeder
    - 0/1 matrisi olarak kaydedilir (bağlı: 1, bağlı değil: 0)
- Namespace: `ProjectYazLab.Services`

**Algorithms.cs**
- Algoritma koordinatörü (Facade Pattern)
- Tüm algoritmaları merkezi olarak yönetir
- Interface'ler üzerinden çalışır (polimorfizm)
- Algoritma örnekleri constructor'da oluşturulur:
  - Pathfinding: BFS, DFS, Dijkstra, A*
  - Analiz: Degree Centrality, Connected Components, Welsh-Powell Coloring
- Metodlar:
  - `RunBFS()`, `RunDFS()`, `RunDijkstra()`, `RunAStar()`: Pathfinding algoritmaları
  - `CalculateDegreeCentrality()`: Merkezilik analizi
  - `GetConnectedComponents()`: Bağlı bileşenleri bulma
  - `RunWelshPowellColoring()`: Renklendirme algoritması (süre ölçümü dahil)
- Kullanıcı arayüzünden tek bir noktadan tüm algoritmalara erişim sağlar
- Namespace: `ProjectYazLab.Services`

#### 4.7.5 Kullanıcı Arayüzü Modülü

**Form1.cs**
- Ana form ve kullanıcı etkileşimleri
- Graf çizimi ve görselleştirme (Panel üzerinde)
- Mouse event'leri:
  - Sol tık: Düğüm seçme/ekleme, kenar oluşturma
  - Sağ tık: Hedef düğüm belirleme
  - Kenar tıklama: Kenar silme
- Algoritma çalıştırma butonları:
  - BFS, DFS, Dijkstra, A* pathfinding algoritmaları
  - Degree Centrality analizi
  - Connected Components analizi
  - Welsh-Powell Coloring algoritması
- Graf yönetimi:
  - CSV dosyası yükleme/kaydetme
  - Komşuluk matrisi kaydetme
  - Düğüm düzenleme/silme
  - Graf sıfırlama
  - Düğümleri çember etrafına yerleştirme
- Görselleştirme:
  - Düğümler: Renkli daireler
  - Kenarlar: Ağırlık bilgisi ile çizgiler
  - Algoritma animasyonları: Adım adım renk değişimleri
- Namespace: `ProjectYazLab`

#### 4.7.6 Modül İlişkileri ve Bağımlılıklar

```
Form1.cs
  ├── Models (Graph, Node, Edge, ColoringResult)
  ├── Services (Algorithms, FileManager)
  └── Interfaces (IFileHandler)

Services/Algorithms.cs
  ├── AlgoModule (Tüm algoritma implementasyonları)
  ├── Interfaces (IGraphAlgorithm, IGraphAnalyzer)
  └── Models (Graph, Node, ColoringResult)

Services/FileManager.cs
  ├── Interfaces (IFileHandler)
  └── Models (Graph, Node, Edge)

AlgoModule/*
  ├── Interfaces (IGraphAlgorithm, IGraphAnalyzer)
  └── Models (Graph, Node, Edge, ColoringResult)
```

#### 4.7.7 Tasarım Desenleri

1. **Facade Pattern**: `Algorithms.cs` sınıfı, tüm algoritma karmaşıklığını gizleyerek basit bir arayüz sunar
2. **Strategy Pattern**: Interface'ler üzerinden farklı algoritma stratejileri uygulanır
3. **Template Method Pattern**: Abstract sınıflar (`AbstractPathfindingAlgorithm`, `AbstractGraphAnalyzer`) ortak algoritma iskeletini tanımlar
4. **Polymorphism**: Interface'ler sayesinde farklı algoritma implementasyonları aynı şekilde kullanılabilir

#### 4.7.8 Modüler Yapının Avantajları

- **Bakım Kolaylığı**: Her modül kendi sorumluluğuna odaklanır
- **Genişletilebilirlik**: Yeni algoritma eklemek için sadece ilgili modüle yeni class eklemek yeterlidir
- **Test Edilebilirlik**: Her modül bağımsız olarak test edilebilir
- **Kod Tekrarının Önlenmesi**: Abstract sınıflar ortak fonksiyonelliği sağlar
- **Separation of Concerns**: Veri yapıları, iş mantığı ve kullanıcı arayüzü ayrılmıştır

### 4.8 İş Akış Diyagramı

```mermaid
flowchart TD
    A[Uygulama Başlat] --> B{Ne yapmak istiyorsun?}
    B -->|Graf Oluştur| C[Boş alana tıkla - Düğüm ekle]
    B -->|Graf Yükle| D[CSV Yükle butonuna tıkla]
    B -->|Algoritma Çalıştır| E[Algoritma seç]
    
    C --> F[İki düğüme tıkla - Kenar ekle]
    F --> G[Graf hazır]
    
    D --> H[CSV dosyası seç]
    H --> I[Graf yüklendi]
    
    E --> J{Algoritma türü?}
    J -->|BFS/DFS| K[Başlangıç düğümü seç]
    J -->|Dijkstra/A*| L[Başlangıç ve hedef düğüm seç]
    J -->|Analiz| M[Analiz butonuna tıkla]
    
    K --> N[Algoritma çalıştır]
    L --> N
    M --> N
    
    N --> O[Sonuçları göster]
    O --> P[Performans ölçümü]
    P --> Q[Görselleştirme]
    
    G --> E
    I --> E
```

---

```

