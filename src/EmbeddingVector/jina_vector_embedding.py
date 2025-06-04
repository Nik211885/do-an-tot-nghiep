import re
import torch
import numpy as np
from typing import List, Dict, Any
from transformers import AutoTokenizer, AutoModel
import warnings

warnings.filterwarnings("ignore")


class SimpleTextChunkerJina:
    def __init__(self, model_name="jinaai/jina-embeddings-v3"):
        self.device = torch.device("cuda" if torch.cuda.is_available() else "cpu")
        print(f"🔧 Đang tải model: {model_name}")

        self.tokenizer = AutoTokenizer.from_pretrained(model_name, trust_remote_code=True)
        self.model = AutoModel.from_pretrained(model_name, trust_remote_code=True)
        self.model.to(self.device)
        self.model.eval()

        self.max_tokens = 256
        print("✅ Model đã sẵn sàng!")

    def count_tokens(self, text: str) -> int:
        return len(self.tokenizer.encode(text, truncation=False))

    def create_chunks(self, text: str) -> List[str]:
        text = re.sub(r'\s+', ' ', text.strip())
        sentences = re.split(r'(?<=[.!?])\s+', text)

        chunks = []
        current_chunk = ""

        for sentence in sentences:
            test_chunk = f"{current_chunk} {sentence}".strip()
            if self.count_tokens(test_chunk) <= self.max_tokens:
                current_chunk = test_chunk
            else:
                if current_chunk:
                    chunks.append(current_chunk)
                current_chunk = sentence

        if current_chunk:
            chunks.append(current_chunk)

        return chunks

    def get_embedding(self, text: str) -> List[float]:
        try:
            inputs = self.tokenizer(
                text,
                return_tensors="pt",
                truncation=True,
                padding=True,
                max_length=self.max_tokens
            )
            inputs = {k: v.to(self.device) for k, v in inputs.items()}

            with torch.no_grad():
                outputs = self.model(**inputs)

                if hasattr(outputs, "pooler_output"):
                    embedding = outputs.pooler_output
                else:
                    embeddings = outputs.last_hidden_state
                    attention_mask = inputs["attention_mask"]
                    mask_expanded = attention_mask.unsqueeze(-1).expand(embeddings.size()).float()
                    sum_embeddings = torch.sum(embeddings * mask_expanded, 1)
                    sum_mask = torch.clamp(mask_expanded.sum(1), min=1e-9)
                    embedding = sum_embeddings / sum_mask

                return embedding.cpu().numpy()[0].tolist()
        except Exception as e:
            print(f"❌ Lỗi tạo embedding: {e}")
            return []

    def process_data(self, text: str) -> List[Dict[str, Any]]:
        print("🔄 Đang chia văn bản...")
        chunks = self.create_chunks(text)
        print(f"📚 Có {len(chunks)} đoạn")

        results = []
        for i, chunk in enumerate(chunks):
            print(f"⚡ Đang xử lý đoạn {i + 1}/{len(chunks)}")
            embedding = self.get_embedding(chunk)

            result = {
                "id": f"chunk_{i}",
                "content": chunk,
                "embedding": embedding,
                "token_count": self.count_tokens(chunk),
                "word_count": len(chunk.split()),
                "char_count": len(chunk),
                "chunk_index": i
            }
            results.append(result)

        print("✅ Xử lý xong tất cả!")
        return results

    def compare_texts(self, text1: str, text2: str) -> float:
        print("🔍 So sánh hai văn bản...")

        embedding1 = self.get_embedding(text1)
        embedding2 = self.get_embedding(text2)

        if not embedding1 or not embedding2:
            return 0.0

        vec1 = np.array(embedding1)
        vec2 = np.array(embedding2)

        dot_product = np.dot(vec1, vec2)
        norm1 = np.linalg.norm(vec1)
        norm2 = np.linalg.norm(vec2)

        if norm1 == 0 or norm2 == 0:
            return 0.0

        return float(dot_product / (norm1 * norm2))


# Demo kiểm thử
def demo():
    chunker = SimpleTextChunkerJina()
    text1 ="""
        Ngay khi Hàn Lập cho rằng loại thời tiết âm u này sẽ kéo dài nữa, mặt trời rốt cục cũng chịu ló ra trên bầu trời.\n\nLúc này cách khoảng thời gian Hàn Lập phát hiện bí mật của lục dịch cũng đã hơn nửa tháng, hắn sớm đã không chờ nổi. Vào đêm hôm ấy, hắn rốt cuốc cũng thấy kỳ quan bốn năm trước thấy được lại phát sinh lần nữa. Một đám dày đặc quang điểm vây quanh bình nhỏ, hình thành một quả cầu sáng lớn.\n\nKhi Hàn Lập vừa nhìn đến tình cảnh này, khối đá nặng trĩu trong lòng hắn cuối cùng cũng được cởi bỏ, cơ bản có thể khẳng định bình nhỏ cũng không phải đồ chỉ dùng được một lần, mà là một kì vật có thể sử dụng nhiều lần.\n\nTiếp tục chờ đợi bảy ngày sau, bình nhỏ này rốt cục lại xuất hiện một giọt lục dịch. Khi Hàn Lập chứng kiến bên trong bình xuất hiện lục dịch, tuy trong lòng đã sớm có tám,chín phần nắm chắc, nhưng vẫn cao hứng dị thường. Cái này cho thấy tương lai mình sẽ có cuồn cuộn không ngừng trân quý dược tài, sẽ không vì thiếu dược vật mà rầu rĩ.\n\nPhải biết rằng độ trân quý của dược tài đa số đều là xem số năm sinh trưởng của nó, thời gian sinh trưởng càng lâu, dược tính lại càng mạnh. Đồng dạng, dược tài có số năm tồn tại càng lâu càng khó kiếm, hơn nữa đều sinh trưởng tại thâm sơn cùng cốc. Không gặp một chút nguy hiểm, đừng mong có thể tìm được.\n\nMặc dù bây giờ có một ít dược điếm, đại phu tự mình chuyên môn bồi dưỡng một ít dược thảo, nhưng đều là những thứ thường dùng, số năm sống cũng rất ngắn nhưng là có thể sử dụng được. Đại bộ phận mọi người sẽ không ngốc mà nuôi dưỡng loại dược tài vài chục năm hay thậm chí khoảng mười năm mới có thể sử dụng.\n\nNhưng cũng có một chút đại phú thế gia vì đề phòng vạn nhất sẽ cho người chuyên môn nuôi dưỡng vài cọng thảo dược phi thường trân quý, để lúc nguy cơ dùng bảo vệ tính mạng, mấy loại dược tài này nếu không qua một thời gia tương đối dài thì không có dược hiệu, bởi vì nhũng thứ bình thường. bằng tài phú những người này có thể dễ dàng mua được, cần gì phải tốn rất nhiều công phu, chuyên môn để bồi dưỡng chứ! Hơn nữa những thế gia với gia tài thật lớn được truyền thừa, cũng không thèm quan tâm tiền bạc tốn hao để bồi thực các loại thảo dược này. Ai cũng không biết mình ngày nào phải sử dụng, cho nên dược thảo này bình thường đều phải trải qua khoảng trăm năm bồi dưỡng. Người bình thường thì không có tài lực và vật lực để làm như vậy.\n\nNgẫu nhiên có một chút dược tài ở ngoài hoang dã xuất hiện, cũng đều bị những thế gia này thu mua. Cái này cũng tạo nên giá tiền của các loại dược tài này cứ thế bị đội lên, còn thường xuyên có cục diện có tiền mà không có hàng.\n\nHàn Lập cũng không xem trọng lần ra ngoài này của Mặc đại phu, phỏng chừng hắn cũng không có quá lớn thu hoạch, nhưng chính mình bây giờ không cần vì thiếu dược tài mà rầu rĩ. Sau khi có cái bình này, bao nhiêu dược tài tốt cũng có thể trong một thời gian ngắn thôi sanh ra.\n\nHàn Lập mang một tâm tình khác thường, trong mười ngày sau, lại phân biệt làm vài lần thí nghiệm thôi sanh thảo dược.\n\nNếu đem lục dịch hòa tan vào nước tưới vào rất nhiều thảo dược, kết quả ngày thứ hai chỉ thu được số dược tài như đã được nuôi dưỡng thêm một, hai năm. Kém rất nhiều so với thảo dược thu được lần đầu tiên. Từ thí nghiệm này, Hàn Lập mơ hồ lĩnh ngộ được một ít quy luật.\n\nTại các thí nghiệm tiếp theo, Hàn Lập dứt khoát tỉnh lược bước hòa tan lục dịch, trực tiếp đem lục dịch nhỏ trên một gốc cây nhân sâm, kết quả ngày thứ hai sau khi tỉnh dậy, Hàn Lập được một cây trăm năm nhân sâm. Thí nghiệm lần này để cho Hàn Lập rất mừng rỡ, không phải bởi vì đã được một loại hi hữu dược tài, mà là bởi vì hắn đã đại khái nắm giữ phương pháp sử dụng lục dịch.\n\nTiếp theo Hàn Lập lại làm vài lần thí nghiệm để bảo tồn lục dịch, đem lục dịch từ trong bình để vào các vật phẩm có thể đựng được như bình sắt, bình ngọc, hồ lô, bình bạc. Phát hiện ra rằng vô luận là loại vật phẩm nào cũng không thể bảo tồn lục dịch quá một khắc, chỉ cần đem lục dịch từ bình nhỏ đi ra, thì trong một khắc phải đem nó dùng hết, nếu không nó sẽ chậm rãi biến mất vô ảnh vô tung. Mà nếu hòa tan với các chất lỏng khác cũng giống giống như vậy, mặc dù có thể để một thời gian hơi dài một chút, nhưng chỉ cần qua nhất định thời gian, ở lại bình đựng chỉ còn lại các chất lỏng khác đã rót vào, thành phần lục dịch vẫn cứ biến mất.\n\nSau khi làm vài lần loại thí nghiệm này, Hàn Lập đối với việc đựng lục dịch bởi các loại vật phẩm khác mất đi hoàn toàn tin tưởng, xem ra không có cách nào đại lượng chứa đυ.ng chất lỏng thần bí này, không thể làm gì khác hơn là đi làm một loại thí nghiệm kiểm tra dược tính.\n\nHàn Lập nhỏ một giọt lục dịch trên một gốc cây tam ô thảo xanh biếc, đem nó biến thành một gốc cây tam ô thảo màu vàng trăm năm tuổi, vài ngày tiếp theo lại nhỏ cho nó thêm một giọt lục dịch, tuổi thọ của nó lại cũng tăng lên thêm một trăm năm.\n\nChứng kiến làm như vây quả thật là có hiệu quả, Hàn Lập trong hai tháng tiếp theo tiếp tục lặp lại cách làm như vậy, mỗi khi có một giọt lục dịch mới từ bình nhỏ sinh ra thì hắn lại đem nó nhỏ vào gốc cây tam ô thảo. Mà gốc cây tam ô thảo cũng không phụ hy vọng, lá cây dần dần từ sắc vàng chuyển sang vàng đen, từ vàng đen chuyển sang đen hẳn, rốt cục sau khi nó biến thành đen bóng. Nó trở thành một gốc cây thế gian hiếm có ngàn năm tam ô thảo.\n\nKiểm tra lần này rất thành công, xem bộ dáng này nếu kiên nhẫn có thể đem tuổi thọ của gốc tam ô thảo tiếp tục tăng lên, nhưng mà việc này đối với Hàn Lập hoàn toàn không cần thiết. Chỉ cần biết cách làm này là hoàn toàn có thể. Bây giờ hắn cũng không cần loại dược tài có tuổi thọ quá lớn, chỉ cần khoảng vài trăm năm dược tài cũng đủ cho hắn phục dụng.\n\nSau khi trải qua hàng loạt các thí nghiệm nhàm chán, Hàn Lập rổt cục có thể rảnh rỗi nghỉ tạm một chút, hảo hảo một phen tính kế, lúc này cách khoảng thời gian từ khi Mặc đại phu xuống núi cũng không ngắn rồi.\n\nBây giờ Hàn Lập cầm trong tay gốc ngàn năm tam ô thảo, nằm tại phòng của chính mình, trên giường gỗ, phát ngốc.\n\nHai mắt hắn thẳng tắp nhìn chằm chằm dược thảo đen thùi, tựa hồ như đang nghiên cứu nó. Nhưng chỉ cần có một người ở trong phòng, từ ánh mắt tán loạn của hắn nhìn ra, tâm tư hắn căn bản không có đặt trên gốc tam ô thảo, mà là đang thần du thiên ngoại, không biết đang suy nghĩ cái gì.\n\nBây giờ hắn hoàn toàn không đắm chìm trong vui sướиɠ vì cây tam ô thảo, mà là đang tinh tế nghĩ đến chỗ tốt cùng nguy hiểm mà bình nhỏ mang đến, vì đường lui của chính mình mà suy nghĩ.\n\nHàn Lập từ các loại sách trong phòng Mặc đại phu chứng kiến không ít các ví dụ\" hoài bích kỳ tội\", cái bình trong tay hắn có thể xưng là bảo vật vô giá, nếu bị người ngoài biết được hắn có một bảo bối như vậy trong tay, hắn tuyệt đối không sống được đến buổi sáng ngày thứ hai. Hắn cùng rất nhiều người \" có ngọc\" trước kia giống nhau, bị bao phủ bởi rất nhiều đồ phu tham lam mà tới. Người ở xa không nói, nói về những người ở gần, nếu mấy vị môn chủ của bổn môn biết bí mật của cái bình, họ nhất định sẽ không buông tha chính mình, sẽ nghĩ mọi phương pháp gϊếŧ người đoạt bảo, mà chính mình sẽ rơi vào thê lương hạ tràng \" Bảo đoạt nhân diệt\".\n\nChính mình tuyệt không thể đem bí mật của bình tử nói cho bất luận kẻ nào, trên núi này cũng cần phải cẩn thận sử dụng, bình nhỏ hấp thu quang điểm gây ra động tĩnh quá lớn, rất có thể sẽ bị ngoại nhân phát hiện bí mật. Hàn Lập hạ quyết tâm thủ khẩu như bình, không tiết lộ ngoại nhân một chữ.\n\n\" Nhưng mà chính mình bây giờ cần dược tài để tu luyện, không dùng bình nhỏ này quá đáng tiếc, chính mình còn cần phải nghĩ ra một biện pháp vẹn toàn cả đôi đường\" Hắn nhớ tới chính mình không thể tiến bước tu luyện, lại có chút buồn bã. Mặc kệ thế nào tiến độ tu luyện khẩu quyết không được chậm trễ, không phải vì sự đốc xúc của Mặc đại phu mà tu luyện, mà là hắn mơ hồ đã nhận thấy được một ít biến hóa bất đồng do tu luyện vô danh khẩu quyết trên người mình.
    """
    text2 = """
        Lại qua một đêm, sáng sớm Hàn Lập vừa mới thức dậy đã hướng dược viên đi đến, muốn quan sát một chút xem mấy cọng dược thảo có biến hóa gì không.\n\nCòn chưa đi đến dược viên, hắn đột nhiên ngửi thấy vài loại hương thơm nồng nặc của dược thảo.\n\nHàn Lập có chút sửng sốt, trong lòng vừa động \"chẳng lẽ là.\"\n\nCước bộ của hắn không khỏi nhanh hơn, rốt cục cũng đã đi tới phía trước mấy cọng dược thảo toát ra mùi thơm mãnh liệt.\n\nĐây chính là vài cọng thảo dược ngày hôm qua? Hàn Lập không dám tin tưởng con mắt của chính mình, lấy tay vỗ mạnh vào vào cái mặt vẫn còn mang theo vài phần ngái ngủ của mình, đến khi cảm thấy chút đau đớn mới đình chỉ hành vi tự ngược chính mình.\n\n\" Lá cây của hoàng long thảo có chút phát tử, khổ liên hoa cũng mở ra tận chín cánh hoa, vỏ của vong ưu quả biến thành sắc đen, ha ha! Ha ha!\" Hàn Lập cung không nhịn được, cho dù bình thường hắn có tâm tĩnh như nước nhưng lúc này cũng không nhịn được ngửa mặt lên trời cười ha hả.\n\n\" Ta gặp đại may mắn, trong một đêm thảo dược chỉ có 1, 2 năm dược tính, tất cả đều biến thành vài chục năm dược tính. Nhìn sắc thái của lá cây này, của quả này, của cánh hoa, của mùi thơm chính là trân quý dược tài đã hoàn toàn thành thục lâu năm.\n\nNếu theo loại phương thức này nuôi dưỡng thảo dược, chính mình chẳng phải muốn có bao nhiêu trân quý dược tài cũng được sao! Hơn nữa nếu dược tài chính mình không dùng được cũng có thể bán cho người khác, cứ như vậy có bao nhiêu bạc có thể thu về.\" Hàn Lập cũng đè nén không được tâm lý hưng phấn kích động, bắt đầu miên man suy nghĩ.\n\nHàn Lập càng nghĩ càng hưng phấn, càng nghĩ càng xa, cảm giác mình đã kiếm được bảo vật thật sự. Hắn đột nhiên té ngã mấy lần, lúc này hắn cũng không có bộ dáng tỉnh táo thường ngày, phương thức biểu đạt kích động giống như một thiếu niên mười bốn tuổi.\n\nMột lúc lâu sau, Hàn Lập mới bắt đầu thanh tỉnh, ý nghĩ khôi phục cơ cảnh như ngày xưa, bắt đầu lo lắng cách giải quyết một ít nan đề từ cái đại may mắn từ trên trời rơi xuống này.\n\nĐầu tiên dược thảo này từ bề ngoài mà nhìn tựa hồ không có vấn đề gì, nhưng thực chất dược chất còn phải kiểm nghiệm lại, chúng nó dù sao là hấp thu cái chất lỏng kì quái này mới biến thành như vậy, ai biết chúng nó có biến dị thành phần bên trong hay không, ngày hôm qua hạ tràng thê lương của con thỏ, chính mắt mình nhìn thấy, chính mình phải cẩn thận vẫn tốt hơn.\n\nTiếp theo lục dịch trong bình nhỏ thần bí đã dùng hết, không biết dị tượng còn có tiếp tục phát sinh hay không, tốt nhất đừng là đồ dùng một lần, để tối mịt phải xác nhận một chút.\n\nNếu hai phương diện trên không có vấn đề gì, mình còn phải nắm giữ cụ thể chi tiết, bước làm của loại thôi sanh dược này, để hoàn toàn có thể khống chế loại phương pháp không thể tưởng tượng này.\n\nHàn Lập suy nghĩ khá lâu, đã đặt ra mấy cái vấn đề cần phải giải quyết, không giải quyết mấy cái nan đề này, cái đại may mắn này đối với mình chỉ là trăng trong bóng nước mà thôi.\n\nSau khi toàn bộ đều lo lắng xong rồi, Hàn Lập bắt đàu hành động.\n\nHắn đi ra ngoài cốc, vào phòng bếp, hỏi quản sự mua hai con thỏ về, cử động này của Hàn Lập làm cho quản sự bếp vừa cao hứng lại vừa buồn bực, thiếu niên này, tại sao lại mua thỏ về, chẳng lẽ hắn muốn chính mình tự tay gϊếŧ thỏ để luyện tập trù nghệ sao?\n\nHàn Lập cũng mặc kệ người khác nghĩ thế nào thì nghĩ, lần này hắn không đem thỏ nhốt ở trong dược viên, mà đem thỏ nhốt trong phòng mình, để tiện quan sát chúng nó biến hóa.\n\nSau đó hắn ra dược viên, đem vài cọng dược thảo được thôi sanh cẩn thận hái trở về, làm thành vài cân thuốc bổ xương, sau đó trộn lẫn vào những loại thức ăn thỏ thích ăn nhất, một ngày ba lần đút cho lũ thỏ ăn, để xem thảo dược này có độc hay không có độc.\n\nLàm xong tất cả, Hàn Lập lo lắng đợi ban đêm tối, lúc mà hắn cảm giác thời gian quả thật dài đằng đẵng, ban đêm y như hắn chờ đợi cũng đã đến.\n\nTrời vừa mới đen lại, Hàn Lập đã chạy ra ngoài phòng đem bình nhỏ từ túi đặt trên mặt đất, chính mình thì tập trung tinh thần chờ đợi biến hóa của bình nhỏ.\n\nMột khắc đã trôi qua, không có động tĩnh gì.\n\nHai khăc đã trôi qua, bình nhỏ còn không có động tĩnh.\n\nĐến canh 3….\n\nTheo thời gian trôi qua, lòng Hàn Lập càng ngày càng trầm xuống, đợi đến tận hửng đông mà bình nhỏ còn không có dị động gì.\n\nHắn uể oải hoàn toàn, bình này chẳng lẽ là đồ dùng một lần? Chẵng lẽ chính mình không có làm đúng cái gì?\n\nHàn Lập cố gắng lên tinh thần, nhìn một chút hoàn cảnh bốn phía.\n\n\" Không có chỗ nào khả nghi, ngoại trừ trời có chút đen\". Hàn Lập tự nhủ.\n\nHắn đột nhiên bất động, ngẩng đầu lên nhìn trời, bầu trời đen mờ mịt, nhìn không thấy được cái gì, \" trời có chút đen\" những lời này điểm tỉnh Hàn Lập.\n\n\" Chẳng lẽ bởi vì là ngày trời âm u, không có ánh trăng chăng?\". Hàn Lập nhớ lại, trước kia khi bình nhỏ phát sinh dị biến đều là tình huống trời quang mây tạnh, có thể nhìn rõ ánh sao và ánh trăng. Mà hôm nay lại là ngày khí trời âm u, mây đen đầy trời.\n\nHàn Lập so sánh một chút, tinh thần thoáng rung lên, lại chứng kiên sắc trời, biết đêm nay sẽ không có gì xảy ra, liền đem bình thu lại, chuẩn bị đợi đến ngày có trăng đem ra thử lại lần nữa.\n\nChính là ngoài ý liệu của Hàn Lập, nửa tháng sau bầu trời chẳng những không quang mây mà còn liên tục mưa phùn, loại khí trời này vẫn tiếp tục cho đến hôm nay.\n\nHàn Lập nhìn bên ngoài mưa phùn, tâm lý nóng nảy phiền muộn, chính mình càng cần khí trời quang đãng, nó lại càng mưa không ngừng, không có chút ý tứ muốn đình chỉ.\n\nHắn quay đầu lại nhìn trong phòng hai con thỏ đang trú mưa. Bộ dáng nghịch ngợm nhanh nhạy của chúng nó càng để cho Hàn Lập buồn bực, từ ngày hai con thỏ này ăn dược vật trộn lẫn thức ăn, chẳng những không có vấn đề gì, còn so với trước kia tinh ranh hơn. Hơn mười ngày nay, Hàn Lập mỗi ngày đều cẩn thận quan sát chúng nó một phen, xác định lũ thỏ không có dấu hiệu trúng độc, ngược lại bởi vì ăn thuốc bồi cân tráng cốt mà càng thêm kiện tráng.\n\nKết quả tốt này chẳng những không để cho Hàn Lập cao hứng lại, mà để cho tâm lý của hắn có chút mất mát. Không có chút biện pháp để cho chính mình bình tĩnh trở lại, đối với hắn mà nói, bình tử có thể tái sinh ra lục dịch nữa hay không, đã thành mấu chốt của hết thảy. Mà cái khí trời khốn kiếp này triền miên hồi lâu làm cho mê đề này không cách nào tháo gỡ, làm cho tâm lý Hàn Lập buồn bực cực kì!
    """
    print(chunker.compare_texts(text1, text2))


if __name__ == "__main__":
    demo()
