function createFlashMarkup(width,height,uri){

 var embed = document.createElement('embed');
 embed.setAttribute('width',width);
 embed.setAttribute('height',height);
 embed.setAttribute('src',uri);

 document.body.appendChild(embed); 
}