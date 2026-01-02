namespace UnityBridge.Crawler.Kuaishou;

/// <summary>
/// 快手 GraphQL 查询语句。
/// </summary>
public static class KuaishouGraphQL
{
    /// <summary>视频搜索查询。</summary>
    public const string SearchQuery = @"
fragment photoContent on PhotoEntity {
  id
  caption
  timestamp
  likeCount
  viewCount
  commentCount
  coverUrl
  webpCoverUrl
  author {
    id
    name
    headerUrl
  }
}
query visionSearchPhoto($keyword: String, $pcursor: String, $page: String, $searchSessionId: String) {
  visionSearchPhoto(keyword: $keyword, pcursor: $pcursor, page: $page, searchSessionId: $searchSessionId) {
    result
    llsid
    webPageArea
    feeds {
      type
      photo {
        ...photoContent
      }
    }
    pcursor
    searchSessionId
  }
}";

    /// <summary>视频详情查询。</summary>
    public const string VideoDetail = @"
query visionVideoDetail($photoId: String, $page: String) {
  visionVideoDetail(photoId: $photoId, page: $page) {
    result
    photo {
      id
      caption
      timestamp
      likeCount
      viewCount
      commentCount
      coverUrl
      webpCoverUrl
      author {
        id
        name
        headerUrl
      }
    }
  }
}";

    /// <summary>评论列表查询。</summary>
    public const string CommentList = @"
query commentListQuery($photoId: String, $pcursor: String) {
  visionCommentList(photoId: $photoId, pcursor: $pcursor) {
    result
    pcursor
    rootComments {
      commentId
      content
      timestamp
      likedCount
      subCommentCount
      author {
        id
        name
        headerUrl
      }
    }
  }
}";

    /// <summary>子评论列表查询。</summary>
    public const string SubCommentList = @"
query visionSubCommentList($photoId: String, $pcursor: String, $rootCommentId: String) {
  visionSubCommentList(photoId: $photoId, pcursor: $pcursor, rootCommentId: $rootCommentId) {
    result
    pcursor
    subComments {
      commentId
      content
      timestamp
      likedCount
      author {
        id
        name
        headerUrl
      }
    }
  }
}";

    /// <summary>创作者信息查询。</summary>
    public const string CreatorProfile = @"
query visionProfile($userId: String) {
  visionProfile(userId: $userId) {
    result
    userProfile {
      ownerCount {
        following
        follower
        photo
      }
      profile {
        user_name
        headurl
        user_text
        gender
      }
    }
  }
}";

    /// <summary>创作者视频列表查询。</summary>
    public const string CreatorVideos = @"
query visionProfilePhotoList($page: String, $pcursor: String, $userId: String) {
  visionProfilePhotoList(page: $page, pcursor: $pcursor, userId: $userId) {
    result
    pcursor
    feeds {
      photo {
        id
        caption
        timestamp
        likeCount
        viewCount
        commentCount
        coverUrl
      }
    }
  }
}";

    /// <summary>首页推荐查询。</summary>
    public const string HomeFeed = @"
query brilliantTypeDataQuery($pcursor: String, $hotChannelId: String, $page: String) {
  brilliantTypeData(pcursor: $pcursor, hotChannelId: $hotChannelId, page: $page) {
    result
    pcursor
    feeds {
      photo {
        id
        caption
        timestamp
        likeCount
        viewCount
        coverUrl
        author {
          id
          name
          headerUrl
        }
      }
    }
  }
}";
}
