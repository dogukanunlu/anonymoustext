import React, { useCallback } from "react";

const PostList = ({ posts, loadMorePosts, allLoaded }) => {
  const handleScroll = useCallback(
    debounce((e) => {
      const { scrollTop, scrollHeight, clientHeight } = e.target;
      if (scrollTop + clientHeight >= scrollHeight - 100 && !allLoaded) {
        loadMorePosts(false); 
      }
    }, 300), 
    [allLoaded, loadMorePosts]
  );

  return (
    <div
      onScroll={handleScroll}
      style={{ height: "500px", overflowY: "auto", border: "1px solid #ccc" }}
    >
      {posts.map((post) => (
        <div key={post.id} style={{ padding: "10px", borderBottom: "1px solid #eee" }}>
          <p>{post.content}</p>
          <small>{new Date(post.timestamp).toLocaleString()}</small>
        </div>
      ))}
      {!allLoaded && <p>Loading more posts...</p>}
      {allLoaded && <p>No more posts to load.</p>}
    </div>
  );
};

export default PostList;

function debounce(func, wait) {
  let timeout;
  return (...args) => {
    clearTimeout(timeout);
    timeout = setTimeout(() => func(...args), wait);
  };
}
