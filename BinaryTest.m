M = zeros([132,131]);
[X, Y] = meshgrid(1:size(M ,2), 1:size(M, 1));
R = sqrt((X-size(M,2)/2).^2 + (Y-size(M,1)/2).^2);
M(R < 10) = 1;
figure; imagesc(M);
fileID = fopen('M.bin','w');
fwrite(fileID,M);
fclose(fileID);

fid= fopen('M.bin','rb');
A = fread(fid, size(M));
fclose(fid);
figure; imagesc(A);