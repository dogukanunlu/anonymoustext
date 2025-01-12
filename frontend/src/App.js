import React, { useState, useEffect } from "react";
import PostInput from "./components/PostInput";
import PostList from "./components/PostList";

const App = () => {
  const [posts, setPosts] = useState([]);
  const [start, setStart] = useState(0); 
  const [allLoaded, setAllLoaded] = useState(false); 

  const fetchPosts = async (reset = false) => {
    try {
      const response = await fetch(`http://localhost:5000/api/text/content?start=${reset ? 0 : start}&count=6`);
      if (response.ok) {
        const newPosts = await response.json();
        
        if (newPosts.length === 0) {
          setAllLoaded(true); 
        } else {
          setPosts((prevPosts) => (reset ? newPosts : [...prevPosts, ...newPosts]));
          setStart((prevStart) => prevStart + 6); 
        }
      }
    } catch (err) {
      console.error("Failed to fetch posts:", err);
    }
  };

  useEffect(() => {
    fetchPosts(true); 
  }, []);

  const handleNewPost = (newPost) => {
    setPosts((prevPosts) => [newPost, ...prevPosts]); 
  };

  return (
    <div style={{ maxWidth: "600px", margin: "0 auto", padding: "20px" }}>
      <h1>Anonymous Text Sharing</h1>
      <PostInput onSubmit={handleNewPost} />
      <PostList posts={posts} loadMorePosts={fetchPosts} allLoaded={allLoaded} />
    </div>
  );
};

export default App;
