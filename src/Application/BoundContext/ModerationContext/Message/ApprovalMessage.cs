using Application.BoundContext.ModerationContext.DomainEventHandler;

namespace Application.BoundContext.ModerationContext.Message;

public static class ApprovalMessage
{
    public static string GetRejectMessage(
        float plagiarismPercentage, 
        float plagiarismThreshold,
        int similarChunks, 
        int totalChunks, 
        List<PlagiarismSource> sourceDocs)
    {
        var sourceDetails = sourceDocs.Any()
            ? string.Join("\n", sourceDocs.Select(s =>
                $"- Tài liệu ID: {s.DocId}, Số đoạn trùng: {s.MatchCount}, Độ tương đồng cao nhất: {(s.MaxSimilarity * 100):F2}%"))
            : "- Không xác định được tài liệu nguồn cụ thể.";

        return $@"Phát hiện đạo văn!
            Tỷ lệ tương đồng: {plagiarismPercentage:F2}% (ngưỡng {plagiarismThreshold}%)
            Số đoạn trùng: {similarChunks}/{totalChunks}

            Nguồn tài liệu tương đồng:
            {sourceDetails}

            Vui lòng chỉnh sửa nội dung trước khi phê duyệt.";
    }

    public const string ApproveMessage = "Văn bản đã được kiểm tra và không có dấu hiệu đạo văn, phê duyệt tự động.";
}
